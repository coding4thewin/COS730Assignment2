using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Original.Classes;
using System.Diagnostics;

namespace Original.Tests
{
    [TestClass]
    public sealed class Original_Tests
    {
        [TestMethod]
        public async Task Original_TimeFullSystemRun()
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
            long submissionId = await Database.SaveSubmission(testFile, connectionString);

            var reviewerManager = new ReviewerManager(configuration);
            var filteredReviewers = await reviewerManager.GetAvailableReviewers(connectionString, researchInstitution);

            foreach (var reviewer in filteredReviewers)
            {
                reviewer.AssignReview(submissionId);
            }

            EvaluationManager.StartEvaluation(filteredReviewers, submissionId, email, connectionString, isUnitTest: true);

            stopwatch.Stop();

            Assert.IsTrue(submissionId > 0);
            System.Diagnostics.Debug.WriteLine($"Total execution time: {stopwatch.ElapsedMilliseconds}ms");
            System.Diagnostics.Debug.WriteLine($"Total execution time: {stopwatch.Elapsed.TotalSeconds}s");
        }
    }
}
