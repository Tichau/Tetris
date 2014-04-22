<?php

$databaseName = 'test';
$profileTable = 'tetris_profiles';
$scoreTable = 'tetris_scores';
$secretKey = "12345";
$verbose = true;

function dbConnect()
{
	global $verbose;
	global $databaseName;

	$hostUrl = 'localhost';
	$databaseLogin = 'admin';
	$databasePassword = '';
	
	try
	{
		$database = new PDO("mysql:host=$hostUrl;dbname=$databaseName", $databaseLogin, $databasePassword);
	}
	catch (Exception $exception)
	{
		$errorMessage = 'ExceptionRaised';
		if ($verbose)
		{
			$errorMessage = $errorMessage . '|' . $exception->getMessage();
		}
		
		die($errorMessage);
	}
	
	return $database;
}
	
function safe($variable)
{
	$variable = addslashes(trim($variable));
	return $variable;
}

function fail($errorMsg)
{
	print $errorMsg;
	exit;
}

?>