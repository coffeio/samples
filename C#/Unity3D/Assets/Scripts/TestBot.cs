using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using UnityEditor;
using SocketIO;
using UnityEngine.Networking;
using System.Text;
using System.Reflection;
using System.Runtime;
using static Unity.Mathematics.Random;

public class TestBot: MonoBehaviour
{
	private SocketIOComponent socket;
	public InputField CLIENT_ID;
	public InputField AccountName;
	public InputField MESSAGE;
	public Text tv_Result;
    public Button btn_Auth;
    public Button btn_ActionTransfer;
    public Button btn_ActionAdd;
    public Button btn_ActionClear;
    public Button btn_GetTable;
    
    void Auth(){
	    if (AccountName.text == "")
	    {
		    tv_Result.text = "Auth failed, enter account name to connect";
		    return;
	    }
	    tv_Result.text = "Auth";
		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
		//string
		json.AddField("client_id", CLIENT_ID.text); //CLIETN_ID , String, length 24, Randomised, Required
		json.AddField("action", "auth"); // Type of action, String, Required
		json.AddField("dapp", "tested"); // Dapp name, String, Required
		json.AddField("user_name", AccountName.text); // User account name, String, Length 12, Required
	    socket.Emit("bot_module", json);
    }
	
	void ActionTransfer(){
		if (AccountName.text == "")
		{
			tv_Result.text = "ActionTransfer failed, enter account name to connect";
			return;
		}
		/*
			{
				"account": "eosio.token",
				"name": "transfer",
				"data": {
					"from": "fromaccount",
					"to": "toaccount",
					"quantity": "1.0000 USDTEST",
					"memo": "my memo",
				}
			}
		*/
        tv_Result.text = "ActionTransfer";
		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
		json.AddField("client_id", CLIENT_ID.text); //CLIETN_ID , String, length 24, Randomised, Required
		json.AddField("action", "confirm_action"); // Type of action, String, Required
		json.AddField("dapp", "testbotunity"); // Dapp name, String, Required
		JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
			data.AddField("account", "tokenrerere1");
			data.AddField("name", "transfer");
			JSONObject data_data = new JSONObject(JSONObject.Type.OBJECT);
			data_data.AddField("from", AccountName.text);
			data_data.AddField("to", "dappsunity3d");
			data_data.AddField("quantity", "1.0000 USDTEST");
			data_data.AddField("memo", "my memo");
			data.AddField("data", data_data);
		json.AddField("data", data); // Data for execute, JSON or JSON.String, String | Object, Required
		socket.Emit("bot_module", json);
    }
	
	void ActionAdd(){
		if (AccountName.text == "")
		{
			tv_Result.text = "ActionAdd failed, enter account name to connect";
			return;
		}
		/*
			{
				"account": "dappsunity3d",
				"name": "injectdata",
				"data": {
					"from": "fromaccount",
					"value": "My Test Inserted Value",
				}
			}
		*/
		tv_Result.text = "ActionAdd";
		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
		json.AddField("client_id", CLIENT_ID.text); //CLIETN_ID , String, length 24, Randomised, Required
		json.AddField("action", "confirm_action"); // Type of action, String, Required
		json.AddField("dapp", "testbotunity"); // Dapp name, String, Required
		JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
		data.AddField("account", "dappsunity3d");
		data.AddField("name", "injectdata");
		JSONObject data_data = new JSONObject(JSONObject.Type.OBJECT);
		data_data.AddField("from", AccountName.text);
		data_data.AddField("value", MESSAGE.text);
		data.AddField("data", data_data);
		json.AddField("data", data); // Data for execute, JSON or JSON.String, String | Object, Required
		socket.Emit("bot_module", json);
	}
	void ActionClear(){
		if (AccountName.text == "")
		{
			tv_Result.text = "ActionClear failed, enter account name to connect";
			return;
		}
		/*
			{
				"account": "dappsunity3d",
				"name": "cleandata",
				"data": {
					"from": "fromaccount"
				}
			}
		*/
		tv_Result.text = "ActionClear";
		JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
		json.AddField("client_id", CLIENT_ID.text); //CLIETN_ID , String, length 24, Randomised, Required
		json.AddField("action", "confirm_action"); // Type of action, String, Required
		json.AddField("dapp", "testbotunity"); // Dapp name, String, Required
		JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
		data.AddField("account", "dappsunity3d");
		data.AddField("name", "cleandata");
		JSONObject data_data = new JSONObject(JSONObject.Type.OBJECT);
		data_data.AddField("from", AccountName.text);
		data.AddField("data", data_data);
		json.AddField("data", data); // Data for execute, JSON or JSON.String, String | Object, Required
		socket.Emit("bot_module", json);
	}

	void GetTable(){
		StartCoroutine(get_table());
	}
	
	IEnumerator get_table()
	{
		Debug.Log("get_table");
		MyClass myObject = new MyClass(true, "dappsunity3d", "dappsunity3d", "testtable", "0", "", "10");
		string data = JsonUtility.ToJson(myObject);
		var request = new UnityWebRequest("http://bp.coffe.io:18888/v1/chain/get_table_rows", "POST");
		byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
		request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		string response = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
		//return JSON as string
		Debug.Log("Return data: " + response);
		/*
		{"rows":[{"account":"supervisor11","seeds":[{"generation":1,"total":0}]
		*/
		tv_Result.text = response;
		request.Dispose();
	}

	void Start(){
		int _stringLength = 25 - 1;
		string randomString = "";
		string[] characters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};
		for (int i = 0; i <= _stringLength; i++) {
			randomString = randomString + characters[UnityEngine.Random.Range(0, characters.Length)];
		}
		CLIENT_ID.text = randomString.ToUpper();
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        socket.On("connect", Connect);
        socket.On(CLIENT_ID.text, Response);
    }
	
    void Update(){
       
    }
	
    public void Response(SocketIOEvent e)
    {
		Debug.Log("[SocketIO] Response");
		Debug.Log(e.data);
		
        //tv_Result.text = e.data->code;

        if (e.data["action"].str == "auth")
        {
	        if (e.data["code"].n == 1){
		        tv_Result.text = "Auth success";
	        }else if (e.data["code"].n == 2){
		        tv_Result.text = "Auth Canceled";
	        }else if (e.data["code"].n == 3){
		        tv_Result.text = "Auth No confirmation";
	        }else if (e.data["code"].n == 4){
		        tv_Result.text = "Auth Repeated authorization in telegram is required!";
	        }else if (e.data["code"].n == 5){
		        tv_Result.text = "Auth Account not find!";
	        }else {
		        tv_Result.text = "Auth Error";
	        }
        }
        if (e.data["action"].str == "confirm_action")
        {
	        if (e.data["code"].n == 1){
		        if (e.data["result"].b == false){
			        tv_Result.text = e.data["message"].str;
		        }else{
			        tv_Result.text = "Action success executed";
		        }
	        }else if (e.data["code"].n == 2){
		        tv_Result.text = "Action Canceled";
	        }else if (e.data["code"].n == 3){
		        tv_Result.text = "Action No confirmation";
	        }else if (e.data["code"].n == 4){
		        tv_Result.text = "Action Repeated authorization in telegram is required!";
	        }else {
		        tv_Result.text = "Action Error";
	        }
        }
		
		/*
			-----------> JavaScript
		if(receive.action == 'auth'){
			if(receive.code == 1){
				//All is ok. receive.result contain account name
				return { result:receive.result, message:'' };
			}else if(receive.code == 2){
				return { result:false, message:'Canceled' };
			}else if(receive.code == 3){
				return { result:false, message:'No confirmation' };
			}else if(receive.code == 4){
				return { result:false, message:'Repeated authorization in telegram is required!' };
			}else if(receive.code == 5){
				return { result:false, message:'Account not find!' };
			}else{
				return { result:false, message:'Error' };
			}
		}
		if(receive.action == 'confirm_action'){
			if(receive.code == 1){
				if(receive.result==false){
					return { result:false, message:receive.message };
				}else{
					//All is ok.
					return { result:true, message:'' };
				}
			}else if(receive.code == 2){
				return { result:false, message:'No confirmation' };
			}else if(receive.code == 3){
				return { result:false, message:'Authorisation Error' };
			}else if(receive.code == 4){
				return { result:false, message:'Repeated authorization in telegram is required!' };
			}else{
				return { result:false, message:'Error' };
			}
		}
		*/
    }
	
    public void Connect(SocketIOEvent e)
    {
		Debug.Log("[SocketIO] Connect");
        tv_Result.text = "Connected to BOT";
    }
	
	void OnEnable(){
        btn_Auth.onClick.AddListener(Auth);
        btn_ActionTransfer.onClick.AddListener(ActionTransfer);
        btn_ActionAdd.onClick.AddListener(ActionAdd);
        btn_ActionClear.onClick.AddListener(ActionClear);
        btn_GetTable.onClick.AddListener(GetTable);
    }

    void OnDisable(){
		btn_Auth.onClick.RemoveListener(Auth);
		btn_ActionTransfer.onClick.RemoveListener(ActionTransfer);
		btn_ActionAdd.onClick.RemoveListener(ActionAdd);
		btn_ActionClear.onClick.RemoveListener(ActionClear);
		btn_GetTable.onClick.RemoveListener(GetTable);
    }
    
    public class JSONObjectBase
    {
        private System.Collections.Generic.Dictionary<string, JSONObjectBase> _JSONProperties = new System.Collections.Generic.Dictionary<string, JSONObjectBase>();

        public System.Collections.Generic.Dictionary<string, JSONObjectBase> JSONProperties
        {
            get { return _JSONProperties; }
            set { _JSONProperties = value; }
        }

        /// <summary>
        /// Will not be used if JSONProperties.Count > 0
        /// </summary>
        public object Value { get; set; }

        public JSONObjectBase Attr(string name)
        {
            return JSONProperties[name];
        }

        public override string ToString()
        {
            return Render();
        }

        public virtual string RenderJSONValue(object val)
        {
            StringBuilder sb = new StringBuilder();
            if (val == null) { return "null"; }
            if (val.GetType() == typeof(bool))
            {
                if (Convert.ToBoolean(val)) { return "true"; }
                return "false";
            }
            Type[] numberArrays = new Type[] {
                    typeof(int[]), typeof(long[]),typeof(double[]),typeof(float[]),typeof(byte[]),typeof(short[]),typeof(decimal[])
                };
            Type[] numbers = new Type[] {
                   typeof(int), typeof(long), typeof(double), typeof(float), typeof(byte),typeof(short),typeof(decimal)
                };
            if (numberArrays.Contains(val.GetType()))
            {
                sb.Append("[");
                bool firstNum = true;
                foreach (object x in (IEnumerable)val)
                {
                    if (firstNum) { sb.Append(x.ToString()); }
                    if (!firstNum) { sb.Append("," + x.ToString()); }
                    firstNum = false;

                }
                sb.Append("]");
            }
            else if (val.GetType() == typeof(string))
            {
                return "\"" + val.ToString() + "\"";
            }
            else if (val as IEnumerable != null)
            {
                sb.Append("[");
                bool firstArr = true;
                foreach (object x in (IEnumerable)val)
                {
                    if (firstArr) { sb.Append(RenderJSONValue(x)); }
                    if (!firstArr) { sb.Append("," + RenderJSONValue(x)); }
                    firstArr = false;
                }
                sb.Append("]");
            }
            else if (val.GetType().IsSubclassOf(typeof(JSONObjectBase)))
            {
                sb.Append(((JSONObjectBase)val).Render());
            }
            else if (numbers.Contains(val.GetType()))
            {
                sb.Append(val.ToString());
            }
            else
            {
                sb.Append("\"" + val.ToString() + "\"");
            }
            return sb.ToString();
        }

        public virtual string Render()
        {
            StringBuilder sb = new StringBuilder();
            bool firstProp = true;
            if (JSONProperties.Count > 0)
            {
                sb.Append("{");
                foreach (KeyValuePair<string, JSONObjectBase> kvp in JSONProperties)
                {
                    if (firstProp)
                    {
                        sb.Append("\"" + kvp.Key + "\":");
                        firstProp = false;
                    }
                    else
                    {
                        sb.Append(",\"" + kvp.Key + "\":");
                    }
                    sb.Append(kvp.Value.Render());
                }
                sb.Append("}");
            }
            else
            {
                return RenderJSONValue(Value);
            }

            var output = sb.ToString();
            output = output.Replace("\r\n", "\\r\\n")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n");
            return output;
        }
    }

    public class MyClass : JSONObjectBase
    {

        public bool json;
        public string code;
        public string scope;
        public string table;
        public string lower_bound;
        public string upper_bound;
        public string limit;

        public MyClass(bool json, string code, string scope, string table, string lower_bound, string upper_bound, string limit)
        {
            this.json = json;
            this.code = code;
            this.scope = scope;
            this.table = table;
            this.lower_bound = lower_bound;
            this.upper_bound = upper_bound;
            this.limit = limit;
        }
    }
}