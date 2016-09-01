using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using com.ethereal.data.JSONFile;
using Boomlagoon.JSON;
using System;
using Assets.Scripts.com.ethereal.display;
using Assets.Scripts.com.ethereal.util;

/** \mainpage Arquitectura ETH
 *
 * \section intro_sec Introduction
 *
 * Arquitectura generada por EtherealGF con el apoyo de la Universidad del Quindio.
 *
 * \section install_sec Installation
 *
 * \subsection step1 Copiar los archivos en la carpeta Scripts de Unity
 * 
 */

namespace Assets.Scripts.com.ethereal.appsSystem
{
    /** 
    *	@author    Andres Herrera <anfho93@gmail.com> 
    * 	@version   1.0 
    * 	@date      Octubre 31 del 2014
    * 
    *	@class 	EthAppsSystem
    *   @brief 	Esta clase se encarga de reportar a un servidor toda la informacion de juego, por medio de la funcion log, para 
    * 	reportar primero verifica que ya se tenga un identificador de descarga, adicionalmente se le alimenta la informacion
    * 	de idJuego y idVersion.
    *
    */

    public class EthAppsSystem
    {
		/**
		*   @brief delegate y event correspondiente
		*/
		public delegate void OnAppSystemReady( bool success );
		private static event OnAppSystemReady OnReady;
		
        /**
        *	@brief Patrón Singleton para mantener la misma instancia de ethAppsSystem en todo el juego.
        */
        public static EthAppsSystem instance;

        /**
        *	@brief Version del software web al que se va a conectar.
        */
        private static string _versionEthAppsSystem = "1.1";

        /**
        *   @brief Asignación de las propiedades de lectura y escritura a la variable _versionEthAppsSystem.
        *
        *   @return El valor de la variable _versionEthAppsSystem.
        */
        public static string VersionEthAppsSystem
        {
            get { return _versionEthAppsSystem; }
            set { _versionEthAppsSystem = value; }
        }

        /**
        *	@brief Escena que llama al ethAppsSystem.
        */
        public MonoBehaviour parent;

        /**
        *	@brief Conjunto de las variables que se han registrado en el sistema web exportadas de la base de datos.
        */
        private Dictionary<string, string> variables = new Dictionary<string, string>();

        /**
        *	@brief ID de la descarga para la instalación actual.
        */
        private string _idDownload = "0";

        /**
        *   @brief Asignación de las propiedades de lectura y escritura a la variable _idDownload.
        *
        *   @return El valor de la variable _idDownload.
        */
        public string IdDownload
        {
            get { return _idDownload; }
            set { _idDownload = value; }
        }
        /**
        *	@brief Id de la sesion para la instalación actual.
        */
        private string _idSession = "-1";

        /**
        *   @brief Asignación de las propiedades de lectura y escritura a la variable _idSession.
        *
        *   @return El valor de la variable _idSession.
        */
        public string IdSession
        {
            get { return _idSession; }
            set { _idSession = value; }
        }
        /**
        *	@brief Version del software de la aplicación.
        */
        private string _idVersion = "0";

        /**
        *   @brief Asignación de las propiedades de lectura y escritura a la variable _idVersion.
        *
        *   @return El valor de la variable _idVersion.
        */
        public string IdVersion
        {
            get { return _idVersion; }
            set { _idVersion = value; }
        }
        /**
        *	@brief Id que se da a la aplicación cuando se registra.
        */
        private string _idApp = "0";

        /**
        *   @brief Asignación de las propiedades de lectura y escritura a la variable _idApp.
        *
        *   @return El valor de la variable _idApp.
        */
        public string IdApp
        {
            get { return _idApp; }
            set { _idApp = value; }
        }

        /**
        *	@brief End point del servicio web.
        */
        private static string _urlEthAppsSystem = "http://ethgame.com/REST/api/v2/";

        /**
        *   @brief Asignación de las propiedades de lectura y escritura a la variable _urlEthAppsSystem.
        *
        *   @return El valor de la variable _urlEthAppsSystem.
        */
        public static string UrlEthAppsSystem
        {
            get { return _urlEthAppsSystem; }
            set { _urlEthAppsSystem = value; }
        }

        /**
        *	@brief Variable que indica si en un dispositivo ha sido iniciada la aplicacion.
        */
        private static bool _initiated = false;
        public bool Initiated
        {
            get { return _initiated; }
            set { _initiated = value; }
        }
        
        /**
         * Constantes
         */
        private const String CLASS_NAME = "EthAppsSystem";

        /**
        *	@brief Método para Instanciar un ethappssystem.
        *	
        *	Este método es el encargado de crear un nuevo ethAppsSystem.
        */
        public EthAppsSystem()
        {
        }

        /**
        *	@brief Método para definir el ethAppsSystem a usar.
        *	
        *	Este método es el encargado de que cuando no haya alguna instancia de ethAppsSystem cree una nueva, 
        *	de lo contrario si ya hay un ethAppsSystem en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de EthAppsSystem. 
        */
        public static EthAppsSystem GetInstance()
        {
            if (EthAppsSystem.instance == null)
            {
                EthAppsSystem.instance = new EthAppsSystem();
            }

            return EthAppsSystem.instance;
        }

        /**
        *	@brief Método usado para iniciar la conexión obteniendo el id de la aplicacion para obtener sus datos como los son 
        *	la version, variables, etc.
        *
        *	Por medio de los JSON se obtienen los datos de configuracion asi como lo es el idApp y en id version, ademas de la 
        *	variables requeridas por la aplicacion. Si la aplicación es abierta por primera vez por un dispositivo, este método 
        *	se encarga de cargar todas las variables requeridas para ejecutarlo.
        *	
        *	@param 	parent Escena que llama a MonoBehaviour requerida para poder inicializar la conexión.
        *
        */
        public static void Init(MonoBehaviour parent, OnAppSystemReady onReadyFn)
        {
			OnReady = onReadyFn;
            string idApp;
            string idVersion;

            EthAppsSystem ethAppsSystem = EthAppsSystem.GetInstance();

            try
            {
                JSONObjectBoom json = JSONFile.GetFile("configEAS");

                idApp = "" + json.GetString("idApp");
                idVersion = "" + json.GetString("idAppVersion");

                ethAppsSystem.LoadVariables(json.GetObject("vars"));
            }
            catch (Exception)
            {
                throw new System.IO.FileNotFoundException("Error!! File configEAS.txt doesn't exists in the Resources Folder");
            }

            ethAppsSystem.parent = parent;
            if (ethAppsSystem._idSession != "-1")
            {
                return;
            }

            ethAppsSystem._idVersion = idVersion;
            ethAppsSystem._idApp = idApp;

            //datos enviados cuando se ingresa por primera vez a la aplicacion
            if (!ethAppsSystem.CheckInitialInfo())
            {
                ethAppsSystem.GetIdDownload();
            }
            else
            {
                ethAppsSystem.GetIdSession();
            }
        }
		
		/**
        *	@brief Método usado para el asignar los valores correspondientes de informacion
        *
        */
		private void GetIdDownload(){
			
			EthAppsSystem ethAppsSystem = EthAppsSystem.GetInstance();
			Assets.Scripts.com.ethereal.util.URLLoader loader = new Assets.Scripts.com.ethereal.util.URLLoader(this.parent);

            Dictionary<string, string> data = new Dictionary<string, string>();
			
            data.Add("idapp",(_idApp));
            data.Add("iddevice",(SystemInfo.deviceUniqueIdentifier));
            data.Add("model",(SystemInfo.deviceModel));
            data.Add("name",(SystemInfo.deviceName));
            data.Add("platformname",("" + Application.platform));
            data.Add("platformVvrsion",(SystemInfo.operatingSystem));
            data.Add("versionEthAppsSystem",(_versionEthAppsSystem));
           
            string addData = "{ ";

			addData += '"'+"systemLanguage"+'"'+':' +'"'+ Application.systemLanguage+'"';
			addData += ','+'"'+"graphicsDeviceVendor"+'"'+':' +'"'+SystemInfo.graphicsDeviceVendor+'"';
			addData += ','+'"'+"processorCount"+'"'+':' +'"'+SystemInfo.processorCount+'"';
			addData += ','+'"'+"supportsAccelerometer"+'"'+':' +'"'+SystemInfo.supportsAccelerometer+'"';
			addData += ','+'"'+"supportsGyroscope"+'"'+':' +'"'+SystemInfo.supportsGyroscope+'"';
			addData += ','+'"'+"supportsLocationService"+'"'+':' +'"'+SystemInfo.supportsLocationService+'"';
			addData += ','+'"'+"supportsVibration"+'"'+':' +'"'+SystemInfo.supportsVibration+'"';
			addData += ','+'"'+"systemMemorySize"+'"'+':' +'"'+SystemInfo.systemMemorySize +'"'+" }";

            data.Add("additionalInfo",(addData));

            loader.OnRespCB += ethAppsSystem.RespGetIdDownload;
            loader.POST(_urlEthAppsSystem + "downloads/id", data);
		}

        /**
        *	@brief Método encargado de enviar un evento sobre mensajes de la aplicación, ya sean de errores o notificaciones.
        *	
        *	@param parent	Escena que llama a ethAppsSystem requerida.
        *	
        */
        public static void Log(MonoBehaviour parent,string category, string subcategory, string label, string value)
        {

            if (!_initiated)
            {
                return;
            }

            EthAppsSystem ethAppsSystem = EthAppsSystem.GetInstance();

            ethAppsSystem.parent = parent;
            ethAppsSystem.ReportLog(category, subcategory , label, value);
        }
		
		/**
        *	@brief Método encargado de enviar un evento sobre mensajes de la aplicación, ya sean de errores o notificaciones,
		*   cuando tiene que ver con la pantalla.
        *	
        *	@param parent	Escena que llama a MonoBehaviour requerida.
		*   @param screen	pantalla que llega por parametro.
        *	
        */
		public static void LogScreen(MonoBehaviour parent, string screen) 
		{
			if(!_initiated)
			{
                return;
            }

			EthAppsSystem ethAppsSystem = EthAppsSystem.GetInstance();
			
			ethAppsSystem.parent = parent;
			ethAppsSystem.ReportScreen(screen);
		}

        /**
        *	@brief Método encargado de obtener una variable de la base de datos, la cual ya se ha registrado.
        *	
        *	@param varName	Nombre de la variable a obtener.
        *
        *	@return Variable obtenida de la base de datos
        */
        public static string GetVariable(string varName)
        {
            return EthAppsSystem.GetInstance().GetVariableFromDictionary(varName);
        }
		
		/**
        *	@brief Método encargado de obtener una variable de la base de datos, la cual ya se ha registrado.
        *	
        *	@param variableName	Nombre de la variable a obtener.
        *
        *	@return Variable obtenida de la base de datos
        */
		public static void ChangeStateVariable(MonoBehaviour parent, string variableName, string variableValue) 
		{
			if(!_initiated)
			{
                return;
            }

			EthAppsSystem ethAppsSystem = EthAppsSystem.GetInstance();
			ethAppsSystem.parent = parent;
			ethAppsSystem.ReportStateVariable(variableName, variableValue);
		}

        /**
        *	@brief Método el cual es la respuesta de las peticiones del idDownload.
        *	
        *	@param success	true o false, si se pudo o no establecer una conexion con el servidor.
        *	@param resp 	Respuesta de peticiones.
        *
        */
        public void RespGetIdDownload(bool success, string resp)
        {
            Debug.Log(success+" | "+resp);
            if (success)
            {
                JSONObjectBoom json = JSONObjectBoom.Parse(resp);

                if (json.GetBoolean("status") == true)
                {
                    _idDownload = "" + json.GetNumber("idDownload");
					if(_idDownload == "" || _idDownload == null || _idDownload == "0")
                    {
                        _idDownload = json.GetString("idDownload");
                    }
                    Debug.Log(_idDownload);
                    PlayerPrefs.SetString("EthDownloadNumber", _idDownload);
                    
                    GetIdSession();
                } else {
					Debug.Log("error");
				}
            }
			else
			{
            	SendReady( false );
            }
            /**
                {
                    'status' => TRUE,
                    'idDownload' => "25"
                }
            */
        }

		/**
        *	@brief Método para establecer si está listo
        *	
		*	@param success	true o false, para establecer y enviar si está o no listo.
        */
		private void SendReady( bool success )
		{
        	if ( OnReady != null )
			{
        		OnReady( success );
        	}
        }
		
        /**
        *	@brief Método para obtener la sesion del dispositivo cada vez que se accede a la aplicación.
        *	
        */
        public void GetIdSession()
        {

            Assets.Scripts.com.ethereal.util.URLLoader loader = new Assets.Scripts.com.ethereal.util.URLLoader(parent);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("idversion", (_idVersion));
            data.Add("idapp", (_idApp));
            data.Add("iddownload", (_idDownload));
            data.Add("versionEthAppsSystem", (_versionEthAppsSystem));

            loader.OnRespCB += RespGetIdSesion;
            loader.POST(_urlEthAppsSystem + "sessions", data);
        }

        /**
        *	@brief Método que envia los datos para la actualización de variables previamente registradas, basados en el idDownload.
        *	
        */
        public void RefreshVariables()
        {
            Assets.Scripts.com.ethereal.util.URLLoader loader = new Assets.Scripts.com.ethereal.util.URLLoader(parent);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("idversion", (_idVersion));
            data.Add("idapp", (_idApp));
			data.Add("idsession", (_idSession));
            data.Add("iddownload", (_idDownload));
            data.Add("versionEthAppsSystem", (_versionEthAppsSystem));
			Debug.Log (data.ToString());
            loader.OnRespCB += RespGetVariables;
            loader.GET(_urlEthAppsSystem + "variables", data);
        }

        /**
        *	@brief Método encargado de registrar un evento.
        *	
        *	@param string category, con la categoría.
		*	@param string subcategory, con la subcategoría.
		*	@param string label, con la etiqueta.
		*	@param string value, con el valor del log.
        *
        */
        public void ReportLog(string category, string subcategory , string label, string value)
        {

            if (!_initiated)
            {
                return;
            }

            Assets.Scripts.com.ethereal.util.URLLoader loader = new Assets.Scripts.com.ethereal.util.URLLoader(parent);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("idversion", (_idVersion));
            data.Add("idapp", (_idApp));
            data.Add("iddownload", (_idDownload));
            data.Add("idsession", (_idSession));
            data.Add("versionEthAppsSystem", (_versionEthAppsSystem));
			data.Add("category", (category));
			data.Add("type", (subcategory));
			data.Add("log", (label));
			data.Add("value", (value));
			
			loader.OnRespCB += RespLog;
            
            loader.POST(_urlEthAppsSystem + "logs", data);
        }
		
		/**
        *	@brief Método encargado de registrar una pantalla.
        *	
        *	@param screen	pantalla a registrar.
        *
        */
		public void ReportScreen(string screen)
		{
			if (!_initiated)
            {
                return;
            }
			
			Assets.Scripts.com.ethereal.util.URLLoader loader = new Assets.Scripts.com.ethereal.util.URLLoader(parent);

			Dictionary<string, string> data = new Dictionary<string, string>();				
			data.Add("idversion", (_idVersion));
			data.Add("idapp", (_idApp));
			data.Add("iddownload", (_idDownload));
			data.Add("idsession", (_idSession));
			data.Add("versionEthAppsSystem", (_versionEthAppsSystem));
			data.Add("screen", (screen));
			
			loader.OnRespCB += RespScreen;
			loader.POST(_urlEthAppsSystem + "screens", data);
		}

		/**
        *	@brief Método encargado de responder una pantalla.
        *	
        *	@param success	si fue o no exitoso.
        *	@param resp	con la respuesta.
        */
		public void RespScreen(bool success, string resp )
		{
			Debug.Log(success+" | "+resp);
			if ( success )
			{
				JSONObjectBoom json = JSONObjectBoom.Parse(resp);

				if(json.GetBoolean("status") == false) 
				{
					GetIdDownload();
				}
			}
		}

		/**
        *	@brief Método encargado de responder un log.
        *	
        *	@param success	si fue o no exitoso.
        *	@param resp	con la respuesta.
        */
		public void RespLog(bool success, string resp )
		{
			Debug.Log(success + " | " + resp);
			if ( success )
			{
				JSONObjectBoom json = JSONObjectBoom.Parse(resp);

				if(json.GetBoolean("status") == false) 
                {
                    GetIdDownload();
                }
			}
		}

		/**
        *	@brief Método encargado de reportar variable de estado
        *	
        *	@param variableName	con el nombre de la variable.
        *	@param value	con el valor.
        */
		public void ReportStateVariable( string variableName, string value )
		{			
			if (!_initiated)
            {
                return;
            }
			
			Assets.Scripts.com.ethereal.util.URLLoader loader = new Assets.Scripts.com.ethereal.util.URLLoader(parent);

			Dictionary<string, string> data = new Dictionary<string, string>();				
			data.Add("idapp", (_idApp));
			data.Add("iddownload",(_idDownload));
			data.Add("versionEthAppsSystem",(_versionEthAppsSystem));
			data.Add("name",(variableName));
			data.Add("value",(value));
			data.Add("idsession",(_idSession));
			Debug.Log(_urlEthAppsSystem + "variables/analitic");
			loader.OnRespCB += RespState;

			loader.POST2(_urlEthAppsSystem + "variables/analitic", data);
		}

		/**
        *	@brief Método encargado de dar respuesta de estado
        *	
        *	@param success	si fue o no exitoso.
        *	@param resp	con la respuesta.
        */
		public void RespState(bool success, string resp )
		{
			Debug.Log("estate: " + success+" | "+resp);
		}

        /**
        *	@brief Método que recibe la respuesta de la peticion de idSesion.
        *	
        *	@param success	true o false, si se pudo o no establecer una conexion con el servidor.
        *	@param resp 	Respuesta de peticiones.
        *
        */
        public void RespGetIdSesion(bool success, string resp)
        {
            _initiated = true;

            Debug.Log(success+" | "+resp);
            if (success)
            {
                JSONObjectBoom json = JSONObjectBoom.Parse(resp);
                
                if (json.GetBoolean("status") == true)
                {
                    _idSession = "" + json.GetString("idsession");
                }

                RefreshVariables();
            }
            else
            {
                SendReady( false );
            }
        }

        /**
        *	@brief Método que recibe la respuesta de la peticion de las variables.
        *	
        *	@param success	true o false, si se pudo o no establecer una conexion con el servidor.
        *	@param resp 	Respuesta de peticiones.
        *
        */
        public void RespGetVariables(bool success, string resp)
        {
            Debug.Log(success+" | "+resp);
            if (success)
            {
                JSONObjectBoom json = JSONObjectBoom.Parse(resp);

                if (json.GetBoolean("status") == true)
                {
                    LoadVariables(json.GetObject("result").GetObject("vars"));
                    SendReady( true );
                }
            }
			else
			{
            	SendReady( false );
            }
        }

        /**
        *	@brief Método que verifica si ya estaba guardado el idDownload.
        *
        *	@return True si el idDownload ya estaba guardado, de lo contrario false.
        */
        public bool CheckInitialInfo()
        {

			_idDownload = "";//PlayerPrefs.GetString("EthDownloadNumber", "");

            if (_idDownload.Length == 0)
            {
                return false;
            }
            
            return true;
        }

        /**
        *	@brief Método que carga las variables del JSON en el sistema.
        *	
        *	@param json 	Json que cargara las variables en el sistema
        *
        */
        public void LoadVariables(JSONObjectBoom json)
        {
            foreach (KeyValuePair<string, JSONValue> currentVar in json)
            {
                if (currentVar.Value.Type == JSONValueType.String)
                {
                    variables[currentVar.Key] = currentVar.Value.Str;
                }
                else
                {
                    variables[currentVar.Key] = "" + currentVar.Value;
                }
            }
        }

        /**
        *	@brief Método que obtiene las variables registradas en el diccionario.
        *	
        *	@param varName Variable a ser obtenida del diccionario
        *
        *	@return Variable encontrada en el diccionario.
        */
        public string GetVariableFromDictionary(string varName)
        {
            string varValue;

            if (variables.TryGetValue(varName, out varValue))
            {
                return varValue;
            }
            else
            {
                return null;
            }
        }

        /**
        *	@brief Método toString de la clase.
        *	
        *	@return String de la clase.
        */
        public override string ToString()
        {
            return CLASS_NAME;
        }
    }
}
