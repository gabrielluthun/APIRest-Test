using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
// HttpListener : classe .NET permettant de communiquer via les protocoles HTTP

#region Création du serveur
HttpListener listener = new HttpListener();
listener.Prefixes.Add("http://localhost:8080/");
listener.Start();
Console.WriteLine("Serveur démarré sur http://localhost:8080/");
#endregion

#region Méthodes de gestion des requêtes
// Boucle infinie pour écouter les requêtes
while (true)
{
    //Contexte : objet qui contient toutes les informations sur la requête (réponse, requête)
    HttpListenerContext context = listener.GetContext();
    ThreadPool.QueueUserWorkItem(objet => RequestHandler(context));

}

//Fonction statique pour récupérer une requête
static void RequestHandler(HttpListenerContext context)
{
    HttpListenerRequest request = context.Request;
    Console.WriteLine($"Requête reçue :  {request.Url} ");

    //Création de l'objet à convertir en JSON
    var dataJson = new
    {
        message = "Quelque chose",
        date = DateTime.Now
    };

    //Créer la réponse
    string jsonResponse = JsonSerializer.Serialize(dataJson);
    byte[] responsesBytes = Encoding.UTF8.GetBytes(jsonResponse);
    context.Response.ContentType = "application/json";
    context.Response.OutputStream.Write(responsesBytes, 0, responsesBytes.Length);
    context.Response.OutputStream.Close();
}
#endregion
