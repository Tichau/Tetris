<?php 
	include("common.php");

	$operation = safe($_POST['operation']);
	$profileName = safe($_POST['profileName']);
	$password = safe($_POST['password']);
	$hash = safe($_POST['hash']);

    $realHash = md5($operation . $profileName . $password . $secretKey); 
	// $realHash = $hash; // TEMP
    if($realHash == $hash) 
	{
		// Test profile name.
		if (!preg_match("/^[a-z0-9_-]{3,16}$/", $profileName))
		{
			die("Fail|The profile name '$profileName' is invalid.");
		}
		
		switch ($operation)
		{
			case "CreateProfile":
				CreateProfile($profileName, $password);
				break;
				
			case "GetProfile":
				GetProfile($profileName, $password);
				break;
				
			case "IsProfileExist":
				IsProfileExist($profileName);
				break;
				
			default:
				die("Fail|Unknown operation '$operation'.");
				break;
		}
	}
	else
	{
		echo "Fail|Md5 mismatch";
	}
	
	function CreateProfile($profileName, $password)
	{
		global $databaseName;
		global $profileTable;
	
		// Test password.
		if (count($password) == 0)
		{
			die("Fail|Invalid password.");
		}
		
		$database = dbConnect();
		
		// Create profile.
		$reponse = $database->query("insert into $databaseName . $profileTable (`index`, `profileName`, `password`) values (null, '$profileName', '$password')");
		
		if (!$reponse)
		{
			$errorMessage = 'DatabaseRequestFailed';
			if ($verbose)
			{
				foreach ($database->errorinfo() as $error) 
				{
					$errorMessage = $errorMessage . '|' . $error;
				}
			}
			
			die($errorMessage);
		}
		
		// Retrieve profile ID.
		$reponse = $database->query("SELECT `index` FROM $databaseName . $profileTable WHERE `profileName` = '$profileName' LIMIT 1");
		
		if (!$reponse)
		{
			$errorMessage = 'DatabaseRequestFailed';
			if ($verbose)
			{
				foreach ($database->errorinfo() as $error) 
				{
					$errorMessage = $errorMessage . '|' . $error;
				}
			}
			
			die($errorMessage);
		}
		
		$data = $reponse->fetch();
						
		$result = "Success|" . $data["index"];
		
		echo $result;
	}
	
	function GetProfile($profileName, $password)
	{
		global $databaseName;
		global $profileTable;
		
		// Test password.
		if (count($password) == 0)
		{
			die("Fail|Invalid password.");
		}
			
		$database = dbConnect();
				
		// Retrieve profile.
		$reponse = $database->query("SELECT `index`, `profileName` FROM $databaseName . $profileTable WHERE `profileName` = '$profileName' AND `password` = '$password' LIMIT 1");
		
		if (!$reponse)
		{
			$errorMessage = 'DatabaseRequestFailed';
			if ($verbose)
			{
				foreach ($database->errorinfo() as $error) 
				{
					$errorMessage = $errorMessage . '|' . $error;
				}
			}
			
			die($errorMessage);
		}
		
		$data = $reponse->fetch();
		if (!$data)
		{
			die("Fail|Invalid login or password.");
		}
		
		$result = "Success|" . $data['index'] . "|" . $data['profileName'];
		
		echo $result;
	}
	
	function IsProfileExist($profileName)
	{
		global $databaseName;
		global $profileTable;
			
		$database = dbConnect();
				
		// Retrieve profile ID.
		$reponse = $database->query("SELECT `index` FROM $databaseName . $profileTable WHERE `profileName` = '$profileName' LIMIT 1");
		
		if (!$reponse)
		{
			$errorMessage = 'DatabaseRequestFailed';
			if ($verbose)
			{
				foreach ($database->errorinfo() as $error) 
				{
					$errorMessage = $errorMessage . '|' . $error;
				}
			}
			
			die($errorMessage);
		}
		
		if ($reponse->fetch())
		{
			$result = "Success|true";
		}
		else
		{
			$result = "Success|false";
		}
		
		echo $result;
	}
?>