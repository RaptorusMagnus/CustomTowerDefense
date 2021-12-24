using System;
using System.IO;
using System.Xml.Serialization;
using CustomTowerDefense.GameEngine;

namespace CustomTowerDefense.Repository
{
    /// <summary>
    /// Base class for all repositories, with common fields and methods.
    /// A repository gives us access to the persistence layer, to store and retrieve information.
    /// Keep in mind that accessing a repository is much slower than memory access,
    /// so load what you need at the beginning and don't use it in the Update loop for instance. 
    /// </summary>
    public class BaseRepository
    {
        //
        //     .-.   .-.     .--.
        //    | OO| | OO|   / _.-' .-.   .-.  .-.   .''.
        //    |   | |   |   \  '-. '-'   '-'  '-'   '..'
        //    '^^^' '^^^'    '--'
        //
        // TODO: make a singleton to handle file access and use a Lock to avoid thread problems.
        
        protected static RepositoryData RepositoryData { get; private set; }
        
        public BaseRepository()
        {
            var repositoryFilename = Path.Combine("Data", "Data.xml");
            var sampleRepositoryFilename = Path.Combine("Data", "Data-sample.xml");

            // We serialize a sample repository to always have a valid example,
            // that can be copied and customized.
            var sampleRepository = RepositoryData.GetSampleObject();
            Serialize(sampleRepository, sampleRepositoryFilename);

            if (RepositoryData == null)
            {
                RepositoryData = DeserializeObject(repositoryFilename);
            }
        }

        private void Serialize(object theObjectToSerialize, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            var serializer = new XmlSerializer(theObjectToSerialize.GetType());
            serializer.Serialize(writer, theObjectToSerialize);
            writer.Close();
        }
        
        private RepositoryData DeserializeObject(string filename)
        {
            var serializer = new XmlSerializer(typeof(RepositoryData));
            using Stream reader = new FileStream(filename, FileMode.Open);
            
            return (RepositoryData)serializer.Deserialize(reader);
        }
    }
}