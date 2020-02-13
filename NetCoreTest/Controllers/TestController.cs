using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.Azure.Storage; // Namespace for CloudStorageAccount
using Microsoft.Azure.Storage.Queue; // Namespace for Queue storage types
using NetCoreTestProject;
using NetCoreTestProject.Common;
using NetCoreTestProject.DataModel;
using NetCoreTestProject.log;
using NetCoreTestProject.Models;
using NetCoreTestProject.Repositories;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        //const string ServiceBusConnectionString = ""; //"Endpoint=sb://https://requestqueue.queue.core.windows.net/testqueue;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[privateKey]";
        const string QueueName = "zsoltmessagequeue";

        private readonly ITestRepository _testRepository;
        private readonly ILog _logger;
        static IQueueClient queueClient;

        public TestController(
            ITestRepository testRepository,
            ILog logger
        )
        {
            _testRepository = testRepository;
            _logger = logger;
        }

        /// <summary>
        /// insert to database test json. 
        /// </summary>
        /// <param name="testWorkDTO"></param>
        /// <returns></returns>
        [Route("SaveTestWork")]
        [HttpPost]
        [AllowAnonymous]
        [STAThread]
        public async Task<ActionResult<Result<string>>> SaveTestWork(TestWorkDTO testWorkDTO)
        {
            _logger.Information($"Enter into method.");
            try
            {
                TestWork testWork = new TestWork();
                if (testWorkDTO.Attributes != null && testWorkDTO.Attributes.Count() > 0)
                {
                    foreach (var item in testWorkDTO.Attributes)
                    {
                        if (_testRepository.ValidInsertAttributeByEmailAddress(testWorkDTO.Email, item))
                        {
                            _testRepository.Add(new TestWork()
                            {
                                Key = testWorkDTO.Key,
                                Email = testWorkDTO.Email,
                                CrDate = DateTime.Now,
                                Attributes = item
                            });
                        }
                    }
                    var result = await _testRepository.UnitOfWork.SaveChangesAsync();
                    var listAttributes = _testRepository.Get10AttributeByEmailAddress(testWorkDTO.Email);
                    if (listAttributes.Count() == 10)
                    {
                        Common.SendMail(testWorkDTO.Email, listAttributes);
                    }
                    _logger.Information($"After save information: {result.ToString()}");
                }
                testWorkDTO.Email = Common.CalculateHash(testWorkDTO.Email);
                _logger.Error($"Input object content: {JsonConvert.SerializeObject(testWorkDTO)}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception. Message: {ex.Message}");
            }
            return Ok("success");            
        }

        /// <summary>
        /// insert to message queue test json-> (I'm sorry, but I couldn't implement it as requested) 
        /// I save to database directly (I don't know the azure function) -> API name: SaveTestWork
        /// </summary>
        /// <param name="testWorkDTO"></param>
        /// <returns></returns>
        [Route("PostToMessageQueue")]
        [HttpPost]
        [AllowAnonymous]
        [STAThread]
        public async Task<ActionResult<Result<string>>> PostJson(TestWorkDTO testWorkDTO)
        {
            _logger.Information($"Enter into method.");
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Startup.MessageQueue);
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue queue = queueClient.GetQueueReference(QueueName);
                queue.CreateIfNotExists();


                if (testWorkDTO != null)
                {
                    //var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(testWorkDTO)));
                    CloudQueueMessage message = new CloudQueueMessage(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(testWorkDTO)));
                    queue.AddMessage(message);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception. Message: {ex.Message}");
            }

            return Ok("success");
        }

        /// <summary>
        /// get csv file by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTestCsv")]
        [AllowAnonymous]
        public IActionResult GetTestCsv(DateTime date)
        {
            _logger.Information($"Enter into method.");
            string result = string.Empty;
            try
            {
                result = _testRepository.GetCSVByDate(date);
                if (string.IsNullOrEmpty(result))
                {
                    _logger.Warning($"Warning: no results");
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                ByteArrayContent content = null;
                byte[] buffer = Encoding.UTF8.GetBytes(result);

                return File
                (
                    buffer,
                    "application/octet-stream",
                    $"csv_{DateTime.Now.ToLongDateString().Replace(".", "_").Replace(" ", "").Replace(":", "_")}.csv"
                );
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception. Message: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}