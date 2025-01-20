<?php
require_once '../db_config.php';

$host = $db_config['host'];
$user = $db_config['user'];
$password = $db_config['password'];
$dbname = $db_config['dbname'];

// Polaczenie z baza danych
$conn = new mysqli($host, $user, $password, $dbname);

// Sprawdz polaczenie
if ($conn->connect_error) {
    file_put_contents('error_log.txt', "Connection failed: " . $conn->connect_error . PHP_EOL, FILE_APPEND);
    die("Connection failed: " . $conn->connect_error);
}

// Odbieranie danych JSON
$data = json_decode(file_get_contents('php://input'), true);

// Walidacja danych wejsciowych
if (!isset($data['nazwa_gracza'])) {
    http_response_code(400);
    echo json_encode(["error" => "Brak wymaganego pola: nazwa_gracza"]);
    exit;
}

$nazwa_gracza = $conn->real_escape_string($data['nazwa_gracza']);
$playtime_h = isset($data['playtime_h']) ? (float)$data['playtime_h'] : 0;
$ilosc_przedm_fabul = isset($data['ilosc_przedm_fabul']) ? (int)$data['ilosc_przedm_fabul'] : 0;
$ilosc_smierci = isset($data['ilosc_smierci']) ? (int)$data['ilosc_smierci'] : 0;
$ilosc_waluty = isset($data['ilosc_waluty']) ? (int)$data['ilosc_waluty'] : 0;
$ostatni_ukon_quest = isset($data['ostatni_ukon_quest']) ? (int)$data['ostatni_ukon_quest'] : 0;
$postep_fabuly = isset($data['postep_fabuly']) ? (float)$data['postep_fabuly'] : 0.0;
// Sprawdzenie, czy gracz istnieje
$sql = "SELECT idGracza, idSave FROM gracze WHERE nazwa_gracza = '$nazwa_gracza'";
$result = $conn->query($sql);
if (!$result) {
    file_put_contents('error_log.txt', "Error in SELECT query: " . $conn->error . PHP_EOL, FILE_APPEND);
}

if ($result->num_rows > 0) {
    // Aktualizacja istniejacego gracza
    $row = $result->fetch_assoc();
    $idGracza = $row['idGracza'];
    $idSave = $row['idSave'];

    $sql_update_gracze = "UPDATE gracze SET playtime_h = playtime_h + $playtime_h, liczba_sesji = liczba_sesji + 1, ostatnia_data_rozgrywki = NOW() WHERE idGracza = $idGracza";
    if (!$conn->query($sql_update_gracze)) {
        file_put_contents('error_log.txt', "Error in UPDATE gracze query: " . $conn->error . PHP_EOL, FILE_APPEND);
    }

    $sql_update_postep = "UPDATE postep_gry SET ilosc_przedm_fabul = $ilosc_przedm_fabul, ilosc_smierci = $ilosc_smierci, ostatni_ukon_quest = $ostatni_ukon_quest, ilosc_waluty = $ilosc_waluty, postep_fabuly = $postep_fabuly WHERE idSave = $idSave";
    if (!$conn->query($sql_update_postep)) {
        file_put_contents('error_log.txt', "Error in UPDATE postep_gry query: " . $conn->error . PHP_EOL, FILE_APPEND);
    }
} else {
    // Tworzenie nowego gracza
    $sql_insert_postep = "INSERT INTO postep_gry (ilosc_przedm_fabul, ilosc_smierci, ostatni_ukon_quest, ilosc_waluty, postep_fabuly) VALUES ($ilosc_przedm_fabul, $ilosc_smierci, $ostatni_ukon_quest, $ilosc_waluty, $postep_fabuly)";
    if (!$conn->query($sql_insert_postep)) {
        file_put_contents('error_log.txt', "Error in INSERT postep_gry query: " . $conn->error . PHP_EOL, FILE_APPEND);
    }

    $idSave = $conn->insert_id;

    $sql_insert_gracze = "INSERT INTO gracze (playtime_h, liczba_sesji, ostatnia_data_rozgrywki, idSave, nazwa_gracza) VALUES ($playtime_h, 1, NOW(), $idSave, '$nazwa_gracza')";
    if (!$conn->query($sql_insert_gracze)) {
        file_put_contents('error_log.txt', "Error in INSERT gracze query: " . $conn->error . PHP_EOL, FILE_APPEND);
    }
}

// Odpowiedz na zadanie
if ($conn->error) {
    http_response_code(500);
    echo json_encode(["error" => $conn->error]);
} else {
    http_response_code(200);
    echo json_encode(["success" => true]);
}

$conn->close();
?>
