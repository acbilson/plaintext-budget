using Newtonsoft.Json;
using PTB.File;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PTB.File.Tests
{
    [TestClass]
    public class FileSchemaTests
    { 
        /*
        [TestMethod]
        public void ReadsCleanSchema()
        {
            // Arrange
            string text = System.IO.File.ReadAllText(@"Data\clean-schema.json");

            // Act
            LedgerSchema result = JsonConvert.DeserializeObject<LedgerSchema>(text);

            // Assert
            Assert.IsNotNull(result.Files);
            Assert.AreEqual(1, result.Files.Length);

            Assert.IsNotNull(result.Columns);
            Assert.AreEqual(7, result.Columns.Length);
        }

        [TestMethod]
        public void CalculatesCorrectByteIndex()
        {
            // Arrange
            string text = System.IO.File.ReadAllText(@"Data\clean-schema.json");
            LedgerSchema result = JsonConvert.DeserializeObject<LedgerSchema>(text);
            int[][] expected = new int[][]
            {
                new int[2] { 0, 10 },
                new int[2] { 11, 12 },
                new int[2] { 13, 25 },
                new int[2] { 26, 46 },
                new int[2] { 47, 97 },
                new int[2] { 98, 113 },
                new int[2] { 114, 115 }
            };

            // Act
            result.CalculateColumnByteIndices();

            // Assert
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i][0], result.Columns[i].ByteIndex[0], $"Expected start index of Column {result.Columns[i].Name} to be {expected[i][0]} but was {result.Columns[i].ByteIndex[0]}");
                Assert.AreEqual(expected[i][1], result.Columns[i].ByteIndex[1], $"Expected end index of Column {result.Columns[i].Name} to be {expected[i][1]} but was {result.Columns[i].ByteIndex[1]}");
            }
        }
    */
    }
}
