using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Fitbit
{
	/// <summary>
	/// FitbitAPI Handles all the calls to fitbit for retrieving the users data.
	/// Each tracker will have it's own API for getting data
	/// </summary>
	public class FitBitAPI : MonoBehaviour
	{
		/// <summary>
		/// Fill in your ConsumerSecret and ClientID for Fitbit
		/// </summary>
		private const string _consumerSecret = "4a394f17a9a7806c7da7eaeff4e0b6b9";
		private const string _clientId = "22DJCT";
		private const string _callbackURL = "http://test.debuginc.com/envdump.php";
		//If you're making an app for Android, fill in your custom scheme here from Fitbit
		//if you don't know how to do the callback through a native browser on a mobile device 
		//http://technicalartistry.blogspot.ca/2016/01/fitbit-unity-oauth-2-and-native.html 
		//can probably help :)
		private const string CustomAndroidScheme = "CALLBACKURL";

		private const string _tokenUrl = "https://api.fitbit.com/oauth2/token";
		private const string _baseGetUrl = "https://api.fitbit.com/1/user/-/";
		private const string _profileUrl = _baseGetUrl + "profile.json/";
		private const string _activityUrl = _baseGetUrl + "activities/" ;

		//Heart Beat
		private string _heartRateUrl = _activityUrl + "heart/date/today/1d/1min.json";

		//Intraday Heart Beat
		//private string _heartRateUrl = _activityUrl + "heart/date/today/1d/1sec/time/" + _earlierTime + "/" + _currentTime + ".json";
		//private string _heartRateUrl = _activityUrl + "heart/date/today/1d/1sec/time/00:00/23:59.json";
		//private string _heartRateUrl = _activityUrl + "heart/date/today/1d/1min/time/00:00/23:59.json";

		private static string _currentTime = GetCurrentTime();
		private static string _earlierTime = GetPreviousTime(5);

		private string _returnCode;
		private WWW _wwwRequest;
		private bool _bGotTheData = false;
		private bool _bFirstFire = true;

		private OAuth2AccessToken _oAuth2 = new OAuth2AccessToken();
		public FitbitData _fitbitData = new FitbitData();

		//Debug String for Android
		private string _statusMessage;

		private string CallBackUrl
		{
			get
			{
				//determine which platform we're running on and use the appropriate url
				if(Application.platform == RuntimePlatform.WindowsEditor)
					return   WWW.EscapeURL(_callbackURL); 
				else if(Application.platform == RuntimePlatform.Android)
				{
					return WWW.EscapeURL(CustomAndroidScheme); 
				}
				else
				{
					return WWW.EscapeURL(CustomAndroidScheme);
				}
			}
		}

		public void Start()
		{
			DontDestroyOnLoad(this);
		}

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                LoginToFitbit();
            }
        }
        private void OnGUI()
		{
			if (!_bGotTheData && !string.IsNullOrEmpty(_statusMessage) && _bFirstFire)
			{
				_bFirstFire = false;
			}
		}

		public void LoginToFitbit()
		{
			//we'll check to see if we have the RefreshToken in PlayerPrefs or not. 
			//if we do, then we'll use the RefreshToken to get the data
			//if not then we will just do the regular ask user to login to get data
			//then save the tokens correctly.

			if (PlayerPrefs.HasKey("FitbitRefreshToken"))
			{
				UseRefreshToken();
			}
			else
			{
				UserAcceptOrDeny();
			}

		}
		public void UserAcceptOrDeny()
		{
			//we don't have a refresh token so we gotta go through the whole auth process.
			var url =
				"https://www.fitbit.com/oauth2/authorize?response_type=code&client_id=" + _clientId + "&redirect_uri=" +
				CallBackUrl +
				"&scope=heartrate%20profile";
			Debug.Log ("url");
			Debug.Log (url);
			//Application.OpenURL(url);
			Debug.Log (Application.absoluteURL);
			var new_url = new WWW (url);
			while (!new_url.isDone)
			{
			}
			//Debug.Log ("new_url");
			//Debug.Log(new_url);
			//Debug.Log ("url text");
			//Debug.Log(new_url.text);
			//System.IO.File.WriteAllText (Application.dataPath + "/JSON/text.txt", new_url.text);
			//Debug.Log (new_url.responseHeaders ["LOCATION"]);
			//new_url.responseHeaders ["LOCATION"] = url;
			//Debug.Log (new_url.responseHeaders ["LOCATION"]);
			//Debug.Log(new_url.responseHeaders ["LOCATION"].GetType());
			Application.OpenURL (new_url.responseHeaders ["LOCATION"]);
			//Debug.Log (new_url.responseHeaders ["LOCATION"]);

			//Debug.Log(Application.srcValue);
			//System.IO.File.WriteAllText (Application.dataPath + "/JSON/srcval.html", Application.srcValue);
			// print(url);
			#if UNITY_EDITOR
			#endif
		}

		public void ClearRefreshCode()
		{
			PlayerPrefs.DeleteKey("FitbitRefreshToken");
			Debug.Log("Refresh Token has been CLEARED!");
		}

		private void UseReturnCode()
		{
			Debug.Log("return code isn't empty");
			//not empty means we put a code in
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(_clientId + ":" + _consumerSecret);
			var encoded = Convert.ToBase64String(plainTextBytes);

			var form = new WWWForm();
			form.AddField("client_id", _clientId);
			form.AddField("grant_type", "authorization_code");
			form.AddField("redirect_uri", WWW.UnEscapeURL(CallBackUrl));
			form.AddField("code", _returnCode);

			var headers = form.headers;
			headers["Authorization"] = "Basic " + encoded;

			_wwwRequest = new WWW(_tokenUrl, form.data, headers);
			StartCoroutine(WaitForAccess(_wwwRequest));

			//DIRTY DIRTY HACK
			while (!_wwwRequest.isDone)
			{
			}

			Debug.Log("Token: " + _wwwRequest.text);
			Debug.Log("parsing token");

			var parsed = new JSONObject(_wwwRequest.text);
			ParseAccessToken(parsed);
			Debug.Log("\nParsed Token: " + _oAuth2.Token);

			//now that we have the Auth Token, Lets use it and get data.
			GetAllData();
			_bGotTheData = true;
		}

		public void UseRefreshToken()
		{
			Debug.Log("Using Refresh Token");
			var plainTextBytes = Encoding.UTF8.GetBytes(_clientId + ":" + _consumerSecret);
			var encoded = Convert.ToBase64String(plainTextBytes);

			var form = new WWWForm();
			form.AddField("grant_type", "refresh_token");
			form.AddField("refresh_token", PlayerPrefs.GetString("FitbitRefreshToken"));

			var headers = form.headers;
			headers["Authorization"] = "Basic " + encoded;

			_wwwRequest = new WWW(_tokenUrl, form.data, headers);
			StartCoroutine(WaitForAccess(_wwwRequest));
			//DIRTY DIRTY HACK
			while (!_wwwRequest.isDone)
			{
			}

			Debug.Log("RefreshToken wwwText: " + _wwwRequest.text);
			//check to see if it's errored or not
			//we have an error and thus should just redo the Auth.
			if(!String.IsNullOrEmpty(_wwwRequest.error))
			{
				PlayerPrefs.DeleteKey("FitbitRefreshToken");
				UserAcceptOrDeny();
				UseReturnCode();
				GetAllData();
			}
			else
			{
				Debug.Log("Using the Auth Token (UseRefreshToken)");
				//no errors so parse the accessToken and update everything :)
				var parsed = new JSONObject(_wwwRequest.text);
				ParseAccessToken(parsed);
				GetAllData();
			}
		}

		public void SetReturnCodeFromAndroid(string code)
		{
			if(string.IsNullOrEmpty(code))
				return;
			//we passed the full URL so we'll have to extract the 
			//We will add 6 to the string lenght to account for "?code="
			_returnCode = code.Substring(CustomAndroidScheme.Length + 6);
			Debug.Log("Return Code is: " + _returnCode);

			UseReturnCode();
		}

		public void SetReturnCode(string code)
		{
			if(string.IsNullOrEmpty(code))
				return;

			_returnCode = code;
			UseReturnCode();
		}
			
		public void GetAllData()
		{
			GetProfileData();
			GetHeartRate();
			BuildProfile();

			//make sure the loading screen is open and change message
			_fitbitData.LastSyncTime = DateTime.Now.ToUniversalTime();
			Debug.Log("LastSyncTime: "+ DateTime.Now.ToUniversalTime().ToString("g"));
		}

		#region GetData
		private void GetProfileData()
		{

			//time for Getting Dataz
			var headers = new Dictionary<string, string>();
			headers["Authorization"] = "Bearer " + _oAuth2.Token;

			_wwwRequest = new WWW(_profileUrl, null, headers);
			Debug.Log("Doing GET Request");
			StartCoroutine(WaitForAccess(_wwwRequest));

			//DIRTY DIRTY HACK
			while (!_wwwRequest.isDone)
			{
			}

			ParseProfileData(_wwwRequest.text);

		}
			
		private void GetHeartRate()
		{
			//time for Getting Dataz
			var headers = new Dictionary<string, string>();
			headers["Authorization"] = "Bearer " + _oAuth2.Token;

			Debug.Log ("Heart Rate URL is: " + _heartRateUrl);
			_wwwRequest = new WWW (_heartRateUrl, null, headers);
			Debug.Log("Doing Heart Rate GET Request");
			StartCoroutine(WaitForAccess(_wwwRequest));

			//DIRTY DIRTY HACK
			while (!_wwwRequest.isDone)
			{
			}

			ParseHeartRateData(_wwwRequest.text);
		}

		private void BuildProfile()
		{
			var imageWWW = new WWW(_fitbitData.ProfileData["avatar"]);
			//DIRTY DIRTY HACK
			while (!imageWWW.isDone)
			{
			}

			Debug.Log(_fitbitData.RawProfileData["fullName"]);

			//we should check to see if there is "data" already
			if(_fitbitData.ProfileData.Count != 0)
			{
				foreach (KeyValuePair<string, string> kvp in _fitbitData.ProfileData)
				{
					if(kvp.Key == "avatar")
						continue;

					//put a space between the camelCase
					var tempKey = Regex.Replace(kvp.Key, "(\\B[A-Z])", " $1");
					//then capitalize the first letter
					UppercaseFirst(tempKey);
				}
			}

			_bGotTheData = true;
		}
		#endregion

		#region Parsing
		private void ParseAccessToken(JSONObject parsed)
		{
			var dict = parsed.ToDictionary();
			foreach (KeyValuePair<string, string> kvp in dict)
			{
				if (kvp.Key == "access_token")
				{
					_oAuth2.Token = kvp.Value;
					PlayerPrefs.SetString("FitbitAccessToken", kvp.Value);
				}
				else if (kvp.Key == "expires_in")
				{
					var num = 0;
					Int32.TryParse(kvp.Value, out num);
					_oAuth2.ExpiresIn = num;

				}
				else if (kvp.Key == "refresh_token")
				{
					_oAuth2.RefreshToken = kvp.Value;
					Debug.Log("REFRESH TOKEN: " + kvp.Value);
					PlayerPrefs.SetString("FitbitRefreshToken", kvp.Value);
					Debug.Log("Token We Just Store: " + PlayerPrefs.GetString("FitbitRefreshToken"));
				}
				else if (kvp.Key == "token_type")
				{
					_oAuth2.TokenType = kvp.Value;
					PlayerPrefs.SetString("FitbitTokenType", kvp.Value);
				}
			}
		}

		private void ParseProfileData(string data)
		{
			Debug.Log("inserting json data into fitbitData.RawProfileData");
			//Debug.LogWarning(data);
			XmlDocument xmldoc = JsonConvert.DeserializeXmlNode(data);

			var doc = XDocument.Parse(xmldoc.InnerXml);


			doc.Descendants("topBadges").Remove();
			foreach (XElement xElement in doc.Descendants())
			{
				//Debug.Log(xElement.Name.LocalName + ": Value:" + xElement.Value);V
				if (!_fitbitData.RawProfileData.ContainsKey(xElement.Name.LocalName))
					_fitbitData.RawProfileData.Add(xElement.Name.LocalName, xElement.Value);
				else
				{
					//Debug.LogWarning("Key already found in RawProfileData: " + xElement.Name.LocalName);
					//if the key is already in the dict, we will just update the value for consistency.
					_fitbitData.RawProfileData[xElement.Name.LocalName] = xElement.Value;
				}

				if (_fitbitData.ProfileData.ContainsKey(xElement.Name.LocalName))
				{
					_fitbitData.ProfileData[xElement.Name.LocalName] = xElement.Value;
				}
			}
		}
			
		private void ParseHeartRateData(string data)
		{
			Debug.Log(data);
			var HeartRateTotal = data;
			Debug.Log("Heart Rate: " + HeartRateTotal);
			Debug.Log(Application.dataPath + "/heartratedata.json");
			System.IO.File.WriteAllText (Application.dataPath + "/JSON/heartratedata.json", data);
			//System.IO.File.WriteAllText (@"C:\Users\Fioger\Desktop\UnityFitbit\heartratedata.json", data);

			//_fitbitData.CurrentHeartRate = ToInt(HeartRateTotal);
		}

		#endregion

		static string UppercaseFirst(string s)
		{
			// Check for empty string.
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			// Return char and concat substring.
			return char.ToUpper(s[0]) + s.Substring(1);
		}

		IEnumerator WaitForAccess(WWW www)
		{
			Debug.Log("waiting for access\n");
			yield return www;
			Debug.Log("Past the Yield \n");
			// check for errors
			if (www.error == null)
			{
				Debug.Log("no error \n");
				Debug.Log("wwwText: " + www.text);
				//Debug.Log("WWW Ok!: " + www.text);
				// _accessToken = www.responseHeaders["access_token"];
			}
			if (www.error != null)
			{
				Debug.Log("\n Error" + www.error);
				Debug.Log(www.error);
			}
			Debug.Log("end of WaitForAccess \n");
		}
			
		public static string GetCurrentTime()
		{
			var hour = DateTime.Now.Hour;
			var minute = DateTime.Now.Minute;
			var new_minute = "";

			if (minute < 10)
				new_minute = minute.ToString ().Insert (0, "0");
			else
				new_minute = minute.ToString();

			Debug.Log ("now");
			Debug.Log(hour.ToString () + ":" + new_minute);
			return hour.ToString() + ":" + new_minute;
		}

		public static string GetPreviousTime(int min)
		{
			var hour = DateTime.Now.Hour;
			var minute = DateTime.Now.Minute;
			var new_minute = "";

			if (minute < min) {
				minute = minute + 60 - min;
				if (hour == 0)
					hour = 23;
				else
					hour -= 1;
			} else
				minute -= min;

			if (minute < 10)
				new_minute = minute.ToString ().Insert (0, "0");
			else
				new_minute = minute.ToString ();

			Debug.Log ("1min");
			Debug.Log(hour.ToString () + ":" + new_minute);
			return hour.ToString () + ":" + new_minute;
		}

		private int ToInt(string thing)
		{
			var temp = 0;
			Int32.TryParse(thing, out temp);
			return temp;
		}

		private double ToDouble(string thing)
		{
			var temp = 0.0;
			Double.TryParse(thing, out temp);
			return temp;
		}

	}

}
