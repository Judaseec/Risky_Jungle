package DemoServer;

import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.URL;
import java.util.ArrayList;
import java.util.Date;
import java.util.LinkedList;
import java.util.List;
import javax.net.ssl.HttpsURLConnection;
import javax.xml.bind.DatatypeConverter;
import org.json.*;
import com.notnoop.apns.*;

/// <summary>
/// The sample class showing how you can send push notifications for different "providers", such as iOS, Google Play and Amazon.
/// </summary>
public class PushNotificator {
//private
	//Please provide the required values. Find more details in the manual: Assets/UTNotifications/Documentation/Manual.pdf
	private static final String GOOGLE_PLAY_API_KEY = null;
	private static final String AMAZON_CLIENT_ID = null;
	private static final String AMAZON_CLIENT_SECRET = null;
	private static final String APN_CERT_PATH = null;
	private static final String APN_CERT_PASSWORD = null;
	private static final String WINDOWS_PACKAGE_SID = null;
	private static final String WINDOWS_CLIENT_SECRET = null;
	
//public
	/// <summary>
	/// Sends a push notification to every registered device.
	/// </summary>
	public static int notifyAll(String title, String text, String serverMessage, String notificationProfile) throws Throwable {
		return notifyItems(Registrator.items(), title, text, serverMessage, notificationProfile);
	}
	
	/// <summary>
	/// Sends a push notification to every device in <c>items</c> list.
	/// </summary>
	public static int notifyItems(List<Registrator.Item> items, String title, String text, String serverMessage, String notificationProfile) throws Throwable {
		int notified = 0;
		
		LinkedList<Registrator.Item> gpItems = new LinkedList<Registrator.Item>();
		LinkedList<Registrator.Item> admItems = new LinkedList<Registrator.Item>();
		LinkedList<Registrator.Item> iosItems = new LinkedList<Registrator.Item>();
		LinkedList<Registrator.Item> wnsItems = new LinkedList<Registrator.Item>();
		
		for (Registrator.Item item : items) {
			if ("GooglePlay".equals(item.provider)) {
				gpItems.add(item);
			} else if ("Amazon".equals(item.provider)) {
				admItems.add(item);
			} else if ("iOS".equals(item.provider)) {
				iosItems.add(item);
			} else if ("Windows".equals(item.provider)) {
				wnsItems.add(item);
			}
		}
		
		notified += notifyGooglePlay(gpItems, title, text, serverMessage, notificationProfile);
		notified += notifyAmazon(admItems, title, text, serverMessage, notificationProfile);
		notified += notifyIOS(iosItems, title, text, serverMessage, notificationProfile);
		notified += notifyWindows(wnsItems, title, text, serverMessage, notificationProfile);
		
		return notified;
	}
	
	/// <summary>
	/// Sends a push notification to Google Play devices.
	/// </summary>
	/// <remarks>
	/// Note that in order to correctly support Unicode characters, <c>title</c>, <c>text</c> and any other text values should be URL-encoded:
	/// <c>title = java.net.URLEncoder.encode(title);</c>
	/// <c>text = java.net.URLEncoder.encode(text);</c>
	/// <c>serverMessage = java.net.URLEncoder.encode(serverMessage);</c>
	/// See also: https://developer.android.com/google/gcm/server.html
	/// </remarks>
	@SuppressWarnings("deprecation")
	public static int notifyGooglePlay(List<Registrator.Item> items, String title, String text, String serverMessage, String notificationProfile) throws Throwable {
		if (items == null || items.size() == 0) {
			return 0;
		}
		
		title = java.net.URLEncoder.encode(title);
		text = java.net.URLEncoder.encode(text);
		serverMessage = java.net.URLEncoder.encode(serverMessage);
		
		//Request data json should look like:
		/*
		{
			"registration_ids":["<id1>", ...],
			"data":
			{
				"title":"<Title>",
				"text":"<Text>",
				"<User data key 1>":"<User data value 1>",
				...
			}
		}
		*/
		JSONObject requestData = new JSONObject();
		
		JSONArray registrationIds = new JSONArray();
		for (Registrator.Item it : items) {
			registrationIds.put(it.getId());
		}
		requestData.put("registration_ids", registrationIds);
		requestData.put("data", prepareData(title, text, serverMessage, notificationProfile));
		
		String requestDataString = requestData.toString();
		
		String httpsURL = "https://android.googleapis.com/gcm/send";
		
		URL url = new URL(httpsURL);
		HttpsURLConnection connection = (HttpsURLConnection)url.openConnection();
		connection.setRequestMethod("POST");
		connection.setRequestProperty("Content-length", String.valueOf(requestDataString.length())); 
		connection.setRequestProperty("Content-Type", "application/json");
		connection.setRequestProperty("Authorization", "key=" + GOOGLE_PLAY_API_KEY);
		connection.setDoOutput(true); 
		connection.setDoInput(true);
		
		DataOutputStream output = new DataOutputStream(connection.getOutputStream());  
		output.writeBytes(requestDataString);
		output.close();

		int responseCode = connection.getResponseCode();
		if (responseCode != 200) {
			String errorContent = readResponse(connection.getErrorStream());             
	        throw new RuntimeException(String.format("ERROR: The request failed with a %d response code, with the following message: %s", responseCode, errorContent));
		} else {
			DataInputStream input = new DataInputStream(connection.getInputStream()); 
			input.close();
		}
		
		return items.size();
	}
	
	/// <summary>
	/// Sends a push notification to Amazon android devices.
	/// </summary>
	/// <remarks>
	/// Note that in order to correctly support Unicode characters, <c>title</c>, <c>text</c> and any other text values should be URL-encoded:
	/// <c>title = java.net.URLEncoder.encode(title);</c>
	/// <c>text = java.net.URLEncoder.encode(text);</c>
	/// <c>serverMessage = java.net.URLEncoder.encode(serverMessage);</c>
	/// See also: https://developer.amazon.com/public/apis/engage/device-messaging/tech-docs/06-sending-a-message
	/// </remarks>
	@SuppressWarnings("deprecation")
	public static int notifyAmazon(List<Registrator.Item> items, String title, String text, String serverMessage, String notificationProfile) throws Throwable {
		if (items == null || items.size() == 0) {
			return 0;
		}
		
		title = java.net.URLEncoder.encode(title);
		text = java.net.URLEncoder.encode(text);
		serverMessage = java.net.URLEncoder.encode(serverMessage);
		
		String token = oauth2GetAuthToken("Amazon", "https://api.amazon.com/auth/O2/token", "messaging:push", AMAZON_CLIENT_ID, AMAZON_CLIENT_SECRET, false);
		
		boolean tokenUpdated = false;
		int notifiedCount = 0;
		for (Registrator.Item it : items) {
			try {
				String id = amazonSendMessageToDevice(it.getId(), token, title, text, serverMessage, notificationProfile);
				
				if (!tokenUpdated && TOKEN_EXPIRED.equals(id)) {
					token = oauth2GetAuthToken("Amazon", "https://api.amazon.com/auth/O2/token", "messaging:push", AMAZON_CLIENT_ID, AMAZON_CLIENT_SECRET, true);
					tokenUpdated = true;
					
					id = amazonSendMessageToDevice(it.getId(), token, title, text, serverMessage, notificationProfile);
				}
				
				++notifiedCount;
				if (id != null) {
					it.setId(id);
				}
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
		
		return notifiedCount;
	}
	
	/// <summary>
	/// Sends a push notification to iOS devices. com.notnoop.apns library is used for it (its source code is provided).
	/// </summary>
	/// <remarks>
	/// Note that iOS Registration Ids are considered as Base64- or HEX- encoded APNS tokens (which are originally binary buffers).
	/// <seealso cref="UTNotifications.Manager.OnSendRegistrationId"/>
	/// See also: https://developer.apple.com/library/ios/documentation/NetworkingInternet/Conceptual/RemoteNotificationsPG/Chapters/CommunicatingWIthAPS.html.
	/// </remarks>
	public static int notifyIOS(List<Registrator.Item> items, String title, String text, String serverMessage, String notificationProfile) throws Throwable {
		if (m_apnsService == null) {
			try {
				//Creates and sets up the ApnsService instance. Note that you don't need to use withSandboxDestination() in production.
				m_apnsService = APNS.newService().withCert(APN_CERT_PATH, APN_CERT_PASSWORD).withSandboxDestination().build();
			} catch (Throwable e) {
				System.err.println("Unable to initialize ApnsService. Please check if APN_CERT_PATH & APN_CERT_PASSWORD values are correct");
				throw e;
			}
		} 
		
		ArrayList<byte[]> tokens = new ArrayList<byte[]>(items.size());
		for (int i = 0; i < items.size(); ++i) {
			tokens.add(decodeToken(items.get(i).getId()));
		}
		
		String sound;
		if (notificationProfile != null && !notificationProfile.isEmpty()) {
			sound = "Data/Raw/" + notificationProfile;
		} else {
			sound = "default";
		}
		
		byte[] payload = APNS.newPayload().alertBody(title + "\n" + text).actionKey(title).sound(sound).customField("server_message", serverMessage).buildBytes();
		m_apnsService.push(tokens, payload);
		
		return items.size();
	}
	
	/// <summary>
	/// Sends a push notification to Windows/Windows Phone devices.
	/// </summary>
	/// <remarks>
	/// Note that in order to correctly support Unicode characters, <c>title</c>, <c>text</c> and any other text values should be URL-encoded:
	/// <c>title = java.net.URLEncoder.encode(title);</c>
	/// <c>text = java.net.URLEncoder.encode(text);</c>
	/// <c>serverMessage = java.net.URLEncoder.encode(serverMessage);</c><br><br>
	/// See also: https://msdn.microsoft.com/en-us/library/windows/apps/hh465435.aspx
	/// </remarks>
	@SuppressWarnings("deprecation")
	public static int notifyWindows(List<Registrator.Item> items, String title, String text, String serverMessage, String notificationProfile) throws Throwable {
		if (items == null || items.size() == 0) {
			return 0;
		}
		
		title = java.net.URLEncoder.encode(title);
		text = java.net.URLEncoder.encode(text);
		serverMessage = java.net.URLEncoder.encode(serverMessage);
		
		String token = oauth2GetAuthToken("Windows", "https://login.live.com/accesstoken.srf", "notify.windows.com", WINDOWS_PACKAGE_SID, WINDOWS_CLIENT_SECRET, false);
		
		boolean tokenUpdated = false;
		int notifiedCount = 0;
		for (Registrator.Item it : items) {
			try {
				String id = notifyWindows(token, it.getId(), title, text, serverMessage, notificationProfile);
				
				if (!tokenUpdated && TOKEN_EXPIRED.equals(id)) {
					token = oauth2GetAuthToken("Windows", "https://login.live.com/accesstoken.srf", "notify.windows.com", WINDOWS_PACKAGE_SID, WINDOWS_CLIENT_SECRET, true);
					tokenUpdated = true;
					
					id = amazonSendMessageToDevice(it.getId(), token, title, text, serverMessage, notificationProfile);
				}
				
				++notifiedCount;
				if (id != null) {
					it.setId(id);
				}
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
		
		return notifiedCount;
	}
	
//private
	//Returns updated registrationId if changed, TOKEN_EXPIRED if expired and null otherwise
	private static String amazonSendMessageToDevice(String registrationId, String accessToken, String title, String text, String serverMessage, String notificationProfile) throws Exception {                                        
	    //JSON payload representation of the message.
	    JSONObject payload = new JSONObject();
	     
	    //Define the key/value pairs for your message content and add them to the
	    //message payload.
	    payload.put("data", prepareData(title, text, serverMessage, notificationProfile));  
	    
	    //Convert the message from a JSON object to a string.
	    String payloadString = payload.toString();
	     
	    //Establish the base URL, including the section to be replaced by the registration
	    //ID for the desired app instance. Because we are using String.format to create
	    //the URL, the %1$s characters specify the section to be replaced.
	    String admUrlTemplate = "https://api.amazon.com/messaging/registrations/%1$s/messages";
	     
	    URL admUrl = new URL(String.format(admUrlTemplate, registrationId));
	      
	    //Generate the HTTPS connection for the POST request. You cannot make a connection
	    //over HTTP.
	    HttpsURLConnection connection = (HttpsURLConnection)admUrl.openConnection();
	    connection.setRequestMethod("POST");
	    connection.setDoOutput(true);
	     
	    //Set the content type and accept headers.
	    connection.setRequestProperty("content-type", "application/json");
	    connection.setRequestProperty("accept", "application/json");
	    connection.setRequestProperty("X-Amzn-Type-Version ", "com.amazon.device.messaging.ADMMessage@1.0");
	    connection.setRequestProperty("X-Amzn-Accept-Type", "com.amazon.device.messaging.ADMSendResult@1.0");
	     
	    //Add the authorization token as a header.
	    connection.setRequestProperty("Authorization", "Bearer " + accessToken);
	                 
	    //Obtain the output stream for the connection and write the message payload to it.
	    OutputStream os = connection.getOutputStream();
	    os.write(payloadString.getBytes(), 0, payloadString.getBytes().length);
	    os.flush();
	    connection.connect();
	    
	    //Obtain the response code from the connection.
	    int responseCode = connection.getResponseCode();
	 
	    //Check if we received a failure response, and if so, get the reason for the failure.
	    if (responseCode != 200) {  
	        if (responseCode == 401) {
	            //If a 401 response code was received, the access token has expired. The token should be refreshed
	            //and this request may be retried.
	        	return TOKEN_EXPIRED;
	        }
	        
	        String errorContent = readResponse(connection.getErrorStream());             
	        throw new RuntimeException(String.format("ERROR: The request failed with a %d response code, with the following message: %s", responseCode, errorContent));
	    } else {
	        //The request was successful. The response contains the canonical Registration ID for the specific instance of your
	        //app, which may be different that the one used for the request.
	         
	        String responseContent = readResponse(connection.getInputStream());          
	        JSONObject parsedObject = new JSONObject(responseContent);
	         
	        String canonicalRegistrationId = parsedObject.getString("registrationID");
	        
	        //Check if the two Registration IDs are different.
	        if (!canonicalRegistrationId.equals(registrationId)) {              
	            //At this point the data structure that stores the Registration ID values should be updated
	            //with the correct Registration ID for this particular app instance.
	        	return canonicalRegistrationId;
	        }
	    }
	     
	    return null;
	}
	
	//Returns updated registrationId if changed (never happens in the current version of WNS though), TOKEN_EXPIRED if expired and null otherwise
	private static String notifyWindows(String accessToken, String registrationId, String title, String text, String serverMessage, String notificationProfile) throws Throwable {
		URL url = new URL(registrationId);
		
		if (!url.getHost().endsWith(".notify.windows.com")) {
			throw new SecurityException("Unexpected WNS channel URI: " + registrationId);
		}
		
		String requestDataString = prepareData(title, text, serverMessage, notificationProfile).toString();
		
		HttpsURLConnection connection = (HttpsURLConnection)url.openConnection();
		connection.setRequestMethod("POST");
		connection.setRequestProperty("Content-Type", "application/octet-stream");
		connection.setRequestProperty("Authorization", "Bearer " + accessToken);
		connection.setRequestProperty("Content-length", String.valueOf(requestDataString.length()));
		connection.setRequestProperty("X-WNS-Type", "wns/raw");
		connection.setRequestProperty("X-WNS-Cache-Policy", "cache");
		connection.setDoOutput(true); 
		connection.setDoInput(true);
		
		DataOutputStream output = new DataOutputStream(connection.getOutputStream());  
		output.writeBytes(requestDataString);
		output.close();

		int responseCode = connection.getResponseCode();
		if (responseCode != 200) {
			if (responseCode == 401) {
	            //If a 401 response code was received, the access token has expired. The token should be refreshed
	            //and this request may be retried.
	        	return TOKEN_EXPIRED;
	        }
			
			String errorContent = readResponse(connection.getErrorStream() != null ? connection.getErrorStream() : connection.getInputStream());
	        throw new RuntimeException(String.format("ERROR: The request failed with a %d response code, with the following message: %s", responseCode, errorContent));
		} else {
			//Success! Current version of WNS never sends the changed registrationId, so no need to check a response content.
			DataInputStream input = new DataInputStream(connection.getInputStream());
			input.close();
			
			return null;
		}
	}
	
	private static JSONObject prepareData(String title, String text, String serverMessage, String notificationProfile) {
		/*
			"data":
			{
				"title":"<Title>",
				"text":"<Text>",
				"<User data key 1>":"<User data value 1>",
				...
			}
		*/
		
		JSONObject data = new JSONObject();
		data.put("title", title);
		data.put("text", text);
		data.put("server_message", serverMessage);
		if (notificationProfile != null && !notificationProfile.isEmpty()) {
	    	data.put("notification_profile", notificationProfile);
	    }
		
		return data;
	}
	
	private static String oauth2GetAuthToken(String provider, String url, String scope, String clientId, String clientSecret, boolean forceUpdateAuthToken) throws Exception {
		if (!forceUpdateAuthToken) {
			String token = Registrator.getOAuth2Token(provider);
			if (token != null) {
				return token;
			}
		}
		
		//Encode the body of your request, including your clientID and clientSecret values.
		String body = "grant_type="    + java.net.URLEncoder.encode("client_credentials", "UTF-8")	+ "&" +
		              "scope="         + java.net.URLEncoder.encode(scope, "UTF-8")     			+ "&" +
		              "client_id="     + java.net.URLEncoder.encode(clientId, "UTF-8")				+ "&" +
		              "client_secret=" + java.net.URLEncoder.encode(clientSecret, "UTF-8");
		   
		//Create a new URL object with the base URL for the access token request.
		URL authUrl = new URL(url);
		  
		//Generate the HTTPS connection. You cannot make a connection over HTTP.
		HttpsURLConnection con = (HttpsURLConnection)authUrl.openConnection();
		con.setDoOutput(true);
		con.setRequestMethod("POST");
		  
		//Set the Content-Type header.
		con.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
		con.setRequestProperty("Charset", "UTF-8");
		
		//Send the encoded parameters on the connection.
		OutputStream os = con.getOutputStream();
		os.write(body.getBytes("UTF-8"));
		os.flush();
		con.connect();
		  
		//Convert the response into a String object.
		String responseContent = readResponse(con.getInputStream());
		   
		//Create a new JSONObject to hold the access token and extract
		//the token from the response.
		JSONObject parsedObject = new JSONObject(responseContent);
		String accessToken = parsedObject.getString("access_token");
		int expiresIn = parsedObject.has("expires_in") ? parsedObject.getInt("expires_in") : (Integer.MAX_VALUE / 1000);
		expiresIn = Math.min(10, expiresIn - 10);	//It took some time to deliver the response
		 
		Date tokenExpires = new Date();
		tokenExpires.setTime(tokenExpires.getTime() + expiresIn * 1000);
		
		Registrator.setOAuth2Token(provider, accessToken, tokenExpires);
		 
		return accessToken;
	}
	
	private static String readResponse(InputStream in) throws Exception
	{
	     InputStreamReader inputStream = new InputStreamReader(in, "UTF-8");
	     BufferedReader buff = new BufferedReader(inputStream);
	       
	     StringBuilder sb = new StringBuilder();
	     String line = buff.readLine();
	     while (line != null) {         
	    	 sb.append(line);
	    	 line = buff.readLine();
	     }
	       
	     return sb.toString();
	}
	
	private static byte[] decodeToken(String token) {
		if (token == null) {
			return null;
		} else if (token.endsWith("=")) {
			return DatatypeConverter.parseBase64Binary(token);
		} else {
			return hexStringToByteArray(token);
		}
	}
	
	private static byte[] hexStringToByteArray(String s) {
	    int len = s.length();
	    byte[] data = new byte[len / 2];
	    for (int i = 0; i < len; i += 2) {
	        data[i / 2] = (byte) ((Character.digit(s.charAt(i), 16) << 4) + Character.digit(s.charAt(i+1), 16));
	    }
	    return data;
	}
	
	private static final String TOKEN_EXPIRED = "TOKEN_EXPIRED";
	private static ApnsService m_apnsService = null;
}
