<?php 
	include("common.php");

	$operation = safe($_POST['operation']);
	$profileId = safe($_POST['profileId']);
	$password = safe($_POST['password']);
	$content = safe($_POST['content']);
	$hash = safe($_POST['hash']);

    $realHash = md5($operation . $profileId . $password . $content . $secretKey); 
	// $realHash = $hash; // TEMP
    if($realHash == $hash) 
	{		
		switch ($operation)
		{
			case "SyncProfile":
				SyncProfile($profileId, $password, $content);
				break;
				
			case "GetHighscores":
				GetHighscores($profileId, $content);
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
	
	function SyncProfile($profileId, $password, $content)
	{
		global $databaseName;
		global $profileTable;
		global $scoreTable;
	
		// Test profile name.
		if ($profileId < 0)
		{
			die("Fail|The profile id '$profileId' is invalid.");
		}
		
		// Test password.
		if (count($password) == 0)
		{
			die("Fail|Invalid password.");
		}
		
		$database = dbConnect();
		
		// Retrieve profile.
		$reponse = $database->query("SELECT `index`, `profileName` FROM $databaseName . $profileTable WHERE `password` = '$password' LIMIT 1");
		
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
		
		if (!$reponse->fetch())
		{
			die("Can't connect to profile $profileId.");
		}
		
		// Insert scores.
		$scores = explode("|", $content);
		foreach ($scores as $score) 
		{
			if (strlen($score) == 0)
			{
				continue;
			}
		
			// Get scores info values.
			$scoreValue = 0;
			$linesValue = 0;
			$startLevelValue = 0;
			
			$scoreData = explode(";", $score);
			
			foreach ($scoreData as $data) 
			{
				if (preg_match("/^Score=[0-9]+$/", $data))
				{
					$scoreValue = preg_replace('/^Score=(.+)$/', '$1', $data);
					continue;
				}
				
				if (preg_match("/^Lines=[0-9]+$/", $data))
				{
					$linesValue = preg_replace('/^Lines=(.+)$/', '$1', $data);
					continue;
				}
				
				if (preg_match("/^StartLevel=[0-9]+$/", $data))
				{
					$startLevelValue = preg_replace('/^StartLevel=(.+)$/', '$1', $data);
					continue;
				}
			}
			
			if ($scoreValue <= 0 || $linesValue <= 0 || $startLevelValue < 0)
			{
				// invalid score.
				continue;
			}
				
			// Insert this score.
			$reponse = $database->query("insert into $databaseName . $scoreTable (`index`, `profileIndex`, `score`, `lines`, `startLevel`) values (null, '$profileId', '$scoreValue', '$linesValue', '$startLevelValue')");
			
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
		}
					
		echo "Success";
	}
	
	function GetHighscores($profileId, $content)
	{
		global $databaseName;
		global $profileTable;
		global $scoreTable;
		global $verbose;
			
		$database = dbConnect();

		// Get options from content.
		$scoreCount = 0;
		
		$options = explode("|", $content);
			
		foreach ($options as $option) 
		{
			if (preg_match("/^Count=[0-9]+$/", $option))
			{
				$scoreCount = preg_replace('/^Count=(.+)$/', '$1', $option);
				continue;
			}
		}
		
		// Retrieve highscores.
		$reponse = $database->query("SELECT `profileName`, `score`, `lines`, `startLevel` 
			FROM $databaseName . $scoreTable Scores JOIN $databaseName . $profileTable Profiles ON Scores.profileIndex = Profiles.index 
			ORDER BY `score` DESC LIMIT $scoreCount");
		
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
		
		// Compute result.
		$result = "Success";
		while ($score = $reponse->fetch())
		{
			$result = $result . '|ProfileName=' . $score['profileName'] . ';Score=' . $score['score'] . ';Lines=' . $score['lines'] . ';StartLevel=' . $score['startLevel'];
		}
		
		echo $result ;
	}
?>