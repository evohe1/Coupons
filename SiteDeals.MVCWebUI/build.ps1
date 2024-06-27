$currentRetryAppPool = 0;
$successAppPool = $false;
do
{
	$statusAppPool = Get-WebAppPoolState -name "DefaultAppPool"
	if ($statusAppPool.Value -eq "Started")
	{
		stop-webapppool "DefaultAppPool"
		$currentRetry = 0;
		$success = $false;
		do{
			$status = Get-WebAppPoolState -name "DefaultAppPool"
			if ($status.Value -eq "Stopped")
			{
					
					echo "AppPool is stopped"
					
					$success = $true;
				}
				Start-Sleep -s 1
				$currentRetry = $currentRetry + 1;
			}
		while (!$success -and $currentRetry -le 15)
		
		$successAppPool = $true;
	}
	Start-Sleep -s 1
	$currentRetryAppPool = $currentRetryAppPool + 1;
}
while (!$successAppPool -and $currentRetryAppPool -le 15)

$currentRetryAppPool = 0;
$successAppPool = $false;
do{
    $statusAppPool = Get-WebsiteState  -name "Kampanyan"
	if ($statusAppPool.Value -eq "Started")
	{
		stop-iissite -Name "Kampanyan" -Confirm: $false
		$currentRetry = 0;
		$success = $false;
		do{
			$status = Get-WebsiteState  -name "Kampanyan"
			if ($status.Value -eq "Stopped")
			{
					echo "App is stopped"
					
					$success = $true;
				}
				Start-Sleep -s 1
				$currentRetry = $currentRetry + 1;
			}
		while (!$success -and $currentRetry -le 15)
            $successAppPool = $true;
        }
        Start-Sleep -s 1
        $currentRetryAppPool = $currentRetryAppPool + 1;
    }
while (!$successAppPool -and $currentRetryAppPool -le 15)

Copy-Item ./SiteDeals.MVCWebUI.publish/* "C:\inetpub\wwwroot" -Recurse -Force

start-webapppool "DefaultAppPool"
start-iissite "Kampanyan"