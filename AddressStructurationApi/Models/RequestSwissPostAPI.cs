using System.Text.Json;

namespace AddressStructurationApi.Models
{

    /*
     * Exemple de format d'entrée
        {
          "request": {
            "ONRP": 0,
            "ZipCode": "",
            "ZipAddition": "",
            "TownName": "",
            "STRID": 0,
            "StreetName": "Route de bure",
            "HouseKey": 0,
            "HouseNo": "61",
            "HouseNoAddition": ""
          },
        "zipOrderMode":1,
        "zipFilterMode":0
        }
     * 
     * Exemple de format de sortie
        {
            "QueryAutoComplete4Result": {
                "AutoCompleteResult": [
                    {
                        "Canton": "JU",
                        "CountryCode": "CH",
                        "HouseKey": "0",
                        "HouseNo": "",
                        "HouseNoAddition": "",
                        "ONRP": "1602",
                        "STRID": "45804",
                        "StreetName": "Route de Bure",
                        "TownName": "Porrentruy",
                        "ZipAddition": "00",
                        "ZipCode": "2900"
                    }
                ],
                "Status": 0
            }
        }
     * 
     */


    public class RequestSwissPostAPI
    {
        public Request request {  get; set; }
        public int zipOrderMode { get; set; }
        public int zipFilterMode { get; set; }

        /// <summary>
        /// Constructeur d'un corps de requête envoyé à l'API de la Poste
        /// </summary>
        /// <param name="zipCode">int, correspond au NPA</param>
        /// <param name="townName">string, correspond à la localité</param>
        /// <param name="streetName">string, correspond à la rue</param>
        /// <param name="houseNo">int, correspond au numéro de rue</param>
        public RequestSwissPostAPI(string zipCode, string townName, string streetName, string houseNo)
        {

            request = new Request
            {
                ONRP = 0,
                ZipCode = zipCode,
                ZipAddition = "",
                TownName = townName,
                STRID = 0,
                StreetName = streetName,
                HouseKey = 0,
                HouseNo = houseNo,
                HouseNoAddition = "",
            };

            zipOrderMode = 1;
            zipFilterMode = 0;
        }
        
        public string toJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public class Request
        {
            public int ONRP { get; set; }
            public string ZipCode { get; set; }
            public string ZipAddition { get; set; }
            public string TownName { get; set; }
            public int STRID { get; set; }
            public string StreetName { get; set; }
            public int HouseKey { get; set; }
            public string HouseNo { get; set; }
            public string HouseNoAddition { get; set; }
        }
    }
}
