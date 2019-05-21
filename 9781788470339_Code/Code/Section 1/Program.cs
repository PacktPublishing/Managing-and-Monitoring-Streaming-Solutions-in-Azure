using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using Microsoft.Azure;
using Microsoft.Azure.Management.Insights;
using Microsoft.Azure.Management.Insights.Models;
using Microsoft.Azure.Management.StreamAnalytics;
using Microsoft.Azure.Management.StreamAnalytics.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.Insights;

namespace ASAMonitoringJob
{
    class Program
    {
        

        static void Main(string[] args)
        {
            string resourceGroupName = "<YOUR AZURE RESOURCE GROUP NAME>";
            string streamAnalyticsJobName = "<YOUR STREAM ANALYTICS JOB NAME>";

            // Get authentication token
            TokenCloudCredentials aadTokenCredentials =
                new TokenCloudCredentials(
                    ConfigurationManager.AppSettings["SubscriptionId"],
                    GetAuthorizationHeader());

            Uri resourceManagerUri = new Uri(ConfigurationManager.AppSettings["ResourceManagerEndpoint"]);

            // Create Stream Analytics and Insights management client
            StreamAnalyticsManagementClient streamAnalyticsClient = new
            StreamAnalyticsManagementClient(aadTokenCredentials, resourceManagerUri);
            InsightsManagementClient insightsClient = new
            InsightsManagementClient(aadTokenCredentials, resourceManagerUri);

            // Get an existing Stream Analytics job
            JobGetParameters jobGetParameters = new JobGetParameters()
            {
                PropertiesToExpand = "inputs,transformation,outputs"
            };
            JobGetResponse jobGetResponse = streamAnalyticsClient.StreamingJobs.Get(resourceGroupName, streamAnalyticsJobName, jobGetParameters);

                      

            // Enable monitoring
            ServiceDiagnosticSettingsPutParameters insightPutParameters = new ServiceDiagnosticSettingsPutParameters()
            {
                Properties = new ServiceDiagnosticSettings()
                {
                    StorageAccountName = "<YOUR STORAGE ACCOUNT NAME>"
                }
            };
            InsightsClient.ServiceDiagnosticSettingsOperations.Put(jobGetResponse.Job.Id, insightPutParameters);

        }

        
        public static string GetAuthorizationHeader()

            {
            AuthenticationResult result = null;
                var thread = new Thread(() =>
                {
                    try
                    {
                        var context = new AuthenticationContext(
                            ConfigurationManager.AppSettings["ActiveDirectoryEndpoint"] +
                            ConfigurationManager.AppSettings["ActiveDirectoryTenantId"]);

                        result = context.AcquireToken(
                            resource: ConfigurationManager.AppSettings["WindowsManagementUri"],
                            clientId: ConfigurationManager.AppSettings["AsaClientId"],
                            redirectUri: new Uri(ConfigurationManager.AppSettings["RedirectUri"]),
                            promptBehavior: PromptBehavior.Always);
                    }
                    catch (Exception threadEx)
                    {
                        Console.WriteLine(threadEx.Message);
                    }
                });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Name = "AcquireTokenThread";
                thread.Start();
                thread.Join();

                if (result != null)
                {
                    return result.AccessToken;
                }

                throw new InvalidOperationException("Failed to acquire token");
            
        }
        }
    }
