 Login-AzureRmAccount

  # Select the Azure subscription you want to use to create the resource group.
 Get-AzureRmSubscription –SubscriptionName “Visual Studio Enterprise with MSDN” | Select-AzureRmSubscription

 # If Stream Analytics has not been registered to the subscription, remove remark symbol below (#) to run the Register-AzureProvider cmdlet to register the provider namespace.
 #Register-AzureRmResourceProvider -Force -ProviderNamespace 'Microsoft.StreamAnalytics'

Get-AzureRMStreamAnalyticsJob

#Get info about all Stream Analytics jobs in the subscription

Get-AzureRMStreamAnalyticsJob -ResourceGroupName PacktDemos 

#Lists all of the inputs are defined in a specified Stream Analytics job

Get-AzureRMStreamAnalyticsJob -ResourceGroupName PacktDemos  -Name vehicletelemetry

#Lists all of the inputs defined in a specified Get-AzureRMStreamAnalyticsJob -ResourceGroupName PacktDemos -Name StreamingJob

Get-AzureRMStreamAnalyticsInput -ResourceGroupName PacktDemos -JobName vehicletelemetry -Name Input

#This PowerShell command returns information about the outputs defined in the job vehicleTelemetry.

Get-AzureRMStreamAnalyticsOutput -ResourceGroupName PacktDemos -JobName vehicletelemetry –Name Output

#Gets information about a specific transformation defined in a Stream Analytics job.
Get-AzureRMStreamAnalyticsTransformation -ResourceGroupName PacktDemos -JobName PAcktASADemo –Name Transformation
#Start Stream Analytics job

Start-AzureRMStreamAnalyticsJob -ResourceGroupName PacktDemos -Name packtasanetdemo -OutputStartMode CustomTime -OutputStartTime 2017-08-27T17:12:12Z