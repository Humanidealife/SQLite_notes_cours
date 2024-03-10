// See https://aka.ms/new-console-template for more information
using System; // espace de nom
using System.IO; // permet de gérer la création des fichiers
using System.Data.SQLite;
using System.Security.Cryptography; // permet de gérer la base de données



namespace SQLite_notes_cours
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
      Console.WriteLine(Environment.MachineName);
      Console.WriteLine(Environment.OSVersion);
      Console.WriteLine(Environment.UserName);
      Console.WriteLine(Environment.TickCount);



      // création de la base de données
      // pour tester si la base de données existe
      string dbPath = "database.sqlite";
      if(!File.Exists(dbPath)) CreateDB();

      //Lancement de la méthode AddData
      // AddData("COUR", "Cédric");

      // Lancement de la méthode ReadAllDta
      // ReadAllData();

      // Lancement de la méthode ReadPrenom
      ReadPrenom("AUPONT");

      // création de la DB
      void CreateDB()
      {
        // création du ficher de la base de données
        SQLiteConnection.CreateFile(dbPath);

        // création de la connexion à la base de données
        SQLiteConnection con = new SQLiteConnection("Data Source=database.sqlite;Veriosn=3;"); // la chaîne d'identification de la base de données

        // ouverture de la connexion
        con.Open();

        // des requêtes SQL
        // dans le string sql on met le code SQL pour des requêtes
        // Création d'une table Clients
        string sql = "CREATE TABLE Clients (nom varchar(20), prenom varchar(20))";

        // création d'une command afin de lancer la requête
        SQLiteCommand command = new SQLiteCommand(sql, con);

        // exécution de la command
        command.ExecuteNonQuery();

        // fermeture de la connexion
        con.Close();
      }

      Console.ReadKey();
    }

    // Création d'une méthode qui permet d'ajouter des données dans la table Clients
    static void AddData(string n, string p)
    {
      // idéal est de mettre la connexion dans une méthode afin de ne pas la répéter
      SQLiteConnection con = new SQLiteConnection("Data Source=database.sqlite;Version=3;");
      con.Open();
      // La façon du cours, mais moins sécurisée
      // string sql = "INSERT INTO Clients (nom, prenom) VALUES ('" + n + "', '" + p + "')";
      // SQLiteCommand command = new SQLiteCommand(sql, con);

      // Une façon plus sécurisée
      string sql = "INSERT INTO Clients (nom, prenom) VALUES (@nom, @prenom)";
      SQLiteCommand command = new SQLiteCommand(sql, con);
      command.Parameters.AddWithValue("@nom", n);
      command.Parameters.AddWithValue("@prenom", p);

      command.ExecuteNonQuery();

      con.Close();
    }

    // Méthode permettant de lire le contenu global d'une table de base de données
    static void ReadAllData()
    {
      SQLiteConnection con = new SQLiteConnection("Data Source=database.sqlite;Version=3;");
      con.Open();
      string sql = "SELECT * FROM Clients";
      SQLiteCommand command = new SQLiteCommand(sql, con);

      SQLiteDataReader reader = command.ExecuteReader();
      while (reader.Read())
      {
        Console.Write("Nom : " + reader.GetString(0));
        Console.WriteLine(" Prénom : " + reader.GetString(1));
      }
      con.Close();
    }

    static void ReadPrenom(string n)
    {
      SQLiteConnection con = new SQLiteConnection("Data Source=database.sqlite;Version=3;");
      con.Open();
      string sql = "SELECT prenom FROM Clients WHERE nom = @nom";

      SQLiteCommand command = new SQLiteCommand(sql, con);

      command.Parameters.AddWithValue("@nom", n);
      SQLiteDataReader reader = command.ExecuteReader();
      reader.Read();

      Console.WriteLine("Prénom : " + reader.GetString(0));
      con.Close();
    }


    // Méthode pour supprimer des données qui a un ID spécifique
    // static void DeleteData(int id)
    // {
    //   SQLiteConnection con = new SQLiteConnection("Data Source=database.sqlite;Version=3;");
    //   con.Open();

    //   string sql = "DELETE FROM Clients WHERE id = @id";
    //   SQLiteCommand command = new SQLiteCommand(sql, con);
    //   command.Parameters.AddWithValue("@id", id);

    //   command.ExecuteNonQuery();

    //   con.Close();
    // }

  }
}
