#######################################################################################################################
# Script require SharePoint CSOM, two dlls: Microsoft.SharePoint.Client.dll and Microsoft.SharePoint.Client.Runtime.dll
# download and install SP CSOM: https://www.microsoft.com/en-us/download/details.aspx?id=35585
#######################################################################################################################


#Specify tenant admin and site URL
$User = "spreader@krutastik.onmicrosoft.com"
$SiteURL = "https://krutastik.sharepoint.com/sites/MSChatBotSurvey"
$XmlLocation = "C:\TFS\Infopulse.MSChatBot.Demo\Infopulse.MSChatBot.Demo\DeploymentScripts"

#add SP CSOM dlls
Add-Type -Path "C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\15\ISAPI\Microsoft.SharePoint.Client.dll"
Add-Type -Path "C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\15\ISAPI\Microsoft.SharePoint.Client.Runtime.dll"

function Main
{

	#get pass
	$password = Read-Host -Prompt "Please enter your password" -AsSecureString
	$creds = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($User,$password)
	
	#get context
	$context = New-Object Microsoft.SharePoint.Client.ClientContext($SiteURL)
	$creds = New-Object Microsoft.SharePoint.Client.SharePointOnlineCredentials($User,$password)
	$context.Credentials = $creds
	
	#Retrieve lists
	$lists = $context.Web.Lists
	$context.Load($lists)
	$context.ExecuteQuery()
	

	$listXmls = GetAllXmls $XmlLocation
	foreach($listXml in $listXmls)
	{
		$templateXml = [xml](get-content $listXml.FullName)

		Write-Output "File: " $listXml.FullName

		#Create list with "custom" list template
		Write-Output "Try add list: " $templateXml.Template.Title
		$listsInfo = New-Object Microsoft.SharePoint.Client.ListCreationInformation
		$listsInfo.Title = $templateXml.Template.Title
		$listsInfo.TemplateType = "100"
		$list = $context.Web.Lists.Add($listsInfo)
		$list.Description = $listTitle
		$list.Update()
		$context.ExecuteQuery()
	    
		Write-Output "List added: " $templateXml.Template.Title

		#add fields
		$fieldCollection = $list.Fields
		foreach ($node in $templateXml.Template.Field) 
		{
			Write-Output "Try add column: " $node.DisplayName
			$type = $node.Type
			if($type -eq "Lookup")
			{
			
				$listId = GetListId $context $node.List
				$node.List = "{$listId}"
			}
			$fieldCollection.AddFieldAsXml($node.OuterXml, $true,[Microsoft.SharePoint.Client.AddFieldOptions]::AddFieldToDefaultView)
		}
		$context.Load($fieldCollection);
		$context.ExecuteQuery();
		Write-Output "Columns added to list: " $templateXml.Template.Title
	}    
}

function GetListId($ctx, $listTitle)
{
	$list = $ctx.Web.Lists.GetByTitle($listTitle);
	$ctx.Load($list)
	$ctx.ExecuteQuery()
	return $list.Id.Guid
}

function GetAllXmls ($location)
{
	$xmls = Get-Childitem $location | Where-Object { $_.Name -like "*.xml" } | Sort-Object
	return $xmls
}
Write-Output "****************************************************Start************************************************************"
Main
Write-Output "****************************************************End**************************************************************"