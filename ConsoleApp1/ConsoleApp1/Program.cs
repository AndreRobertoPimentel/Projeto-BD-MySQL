[Ontem 21:36] ANDRÉ ROBERTO PIMENTEL

using System;

using MySql.Data.MySqlClient;

 

namespace Connection

{

    public class MySqlConnectionManager

    {

        private string connectionString;



        public MySqlConnectionManager(string server, int port, string database, string user, string password)

        {

            connectionString = $"Server={server};Port={port};Database={database};User={user};Password={password};";

        }



        public MySqlConnection CreateConnection()

        {

            return new MySqlConnection(connectionString);

        }

    }



    class Program

    {

        static void Main(string[] args)

        {

            string server = "";

            int port = 3306;

            string database = "loja3Irmas";

            string user = "";

            string password = "";



            MySqlConnectionManager connectionManager = new MySqlConnectionManager(server, port, database, user, password);



            try

            {

                using (MySqlConnection connection = connectionManager.CreateConnection())

                {

                    connection.Open();

                    Console.WriteLine("Conexão com o MySQL estabelecida com sucesso.");



                    bool sair = false;

                    while (!sair)

                    {

                        Console.WriteLine("\nMenu:");

                        Console.WriteLine("1. Cadastrar Produto");

                        Console.WriteLine("2. Pesquisar Produto");

                        Console.WriteLine("3. Sair");



                        Console.Write("Escolha uma opção (1/2/3): ");

                        string escolha = Console.ReadLine();



                        switch (escolha)

                        {

                            case "1":

                                Console.Write("Digite o nome do produto: ");

                                string nomeProduto = Console.ReadLine();

                                Console.Write("Digite o preço do Produto: ");

                                float precoProduto = float.Parse(Console.ReadLine());

                                CadastrarProdutos(connection, nomeProduto, precoProduto);

                                Console.WriteLine($"Produto '{nomeProduto}' cadastrado com sucesso!");

                                break;



                            case "2":

                                Console.Write("Digite o nome do produto a ser pesquisado: ");

                                string nomeProdutoPesquisa = Console.ReadLine();

                                PesquisarProdutos(connection, nomeProdutoPesquisa);

                                break;



                            case "3":

                                sair = true;

                                break;



                            default:

                                Console.WriteLine("Opção inválida. Por favor, escolha 1, 2 ou 3.");

                                break;

                        }

                    }

                }

            }

            catch (MySqlException ex)

            {

                Console.WriteLine($"Erro na conexão: {ex.Message}");

            }



            Console.WriteLine("Pressione qualquer tecla para sair...");

            Console.ReadKey();

        }



        static void CadastrarProdutos(MySqlConnection connection, string nomeProduto, float precoProduto)

        {

            try

            {

                string query = "INSERT INTO Produtos (nomeProduto, precoProduto) VALUES (@nomeProduto, @precoProduto)";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@nomeProduto", nomeProduto);

                command.Parameters.AddWithValue("@precoProduto", precoProduto);

                command.ExecuteNonQuery();

            }

            catch (MySqlException ex)

            {

                Console.WriteLine($"Erro ao cadastrar Produto: {ex.Message}");

            }

        }



        static void PesquisarProdutos(MySqlConnection connection, string nomeProduto)

        {

            try

            {

                string query = "SELECT nomeProduto, precoProduto FROM Produtos WHERE nomeProduto = @nomeProduto";

                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@nomeProduto", nomeProduto);



                using (MySqlDataReader reader = command.ExecuteReader())

                {

                    Console.WriteLine("Produtos encontrados:");

                    while (reader.Read())

                    {

                        string nomeProdutoResultado = reader.GetString("nomeProduto");

                        float precoProdutoResultado = reader.GetFloat("precoProduto");



                        Console.WriteLine($"Nome: {nomeProdutoResultado}, Preço: {precoProdutoResultado}");

                    }

                }

            }

            catch (MySqlException ex)

            {

                Console.WriteLine($"Erro ao pesquisar Produto: {ex.Message}");

            }

        }

    }

}