using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Optimised.Classes;
using System.Diagnostics;

namespace Optimised.Tests
{
    [TestClass]
    public sealed class Optimised_Tests
    {
        [TestMethod]
        public async Task Optimised_TimeFullSystemRun()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");


            var fileContent = "test";
            var fileName = "testfile.pdf";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
            var testFile = new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            string researchInstitution = "University of Pretoria";
            string email = "u26856230@tuks.co.za";

            var stopwatch = Stopwatch.StartNew();
            var submission = Submission.Create(testFile);
            await Database.Save(submission, connectionString);

            EvaluationManager.StartEvaluation(submission.Id, email, researchInstitution, connectionString, isUnitTest: true);

            stopwatch.Stop();

            Assert.IsTrue(submission.Id > 0);
            System.Diagnostics.Debug.WriteLine($"Total execution time: {stopwatch.ElapsedMilliseconds}ms");
            System.Diagnostics.Debug.WriteLine($"Total execution time: {stopwatch.Elapsed.TotalSeconds}s");
        }
    }
}
