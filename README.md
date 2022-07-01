# Mobilny Atlas - Prototyp

Aplikacja umożliwia wizualizację danych kraju po najechaniu telefonem. Wykorzystuje kolorową mapę Europy. Po wykryciu danego kraju pojawia się nazwa kraju, która działa jako przycisk. Po jego kliknięciu wyświetlane jest okno z dodatkowymi informacjami. W ramach projektu przedstawiony został prototyp zawierający 10 krajów do możliwych do detekcji i odczytania.

Aplikacja została napisana z wykorzystaniem pakietu Vuforia. Mapa została podzielona na strefy wykrywane jako osobne obrazy (będącymi fragmentami przyjaznymi wykrywaniu). Jest wiele elementów interaktywnych przypisanych do jednego fragmentu wykrywalnego mapy.

### Wymagania funkcjonalne
- Możliwość wyboru kraju za pomocą interaktywnego UI
- Wyświetlenie zbioru podstawowych informacji o danym państwie

### Wymagania niefunkcjonalne
- Kraje odczytywane z mapy Europy
- 10 krajów możliwych do detekcji (dla wersji demonstracyjnej)
- Aplikacja przeznaczona na telefony wspierające Vuforię

## Instalacja i uruchamianie

Instalacja aplikacji na urządzenia Android odbywa się przez narzędzie adb. Po połączeniu urządzenia z komputerem przez kabel USB wywoływana jest komenda adb z linii poleceń. Użyto przełącznika -r w przypadku istnienia zainstalowanej poprzedniej wersji aplikacji w celu jej nadpisania.

adb install -r {nazwa_pliku}.apk

Po dokonaniu pomyślnej instalacji możliwe jest uruchomienie aplikacji z poziomu telefonu.

## Działanie aplikacji
W celu poprawnego działania aplikacji należy najechać kamerą telefonu na wydrukowaną mapę Europy, którą można znaleźć pod podanym linkiem: [źródło](https://commons.wikimedia.org/wiki/File:Europe_bluemarble_laea_location_map.jpg). Poruszając telefonem nad odpowiednim krajem wyświetli się pinezka z możliwością stuknięcia na przycisk znajdujący się na dole ekranu, który przedstawia zwięzły zbiór informacji o kraju.

## Wygląd aplikacji

<p align="center">
<br>
<img src="/images/Screenshot_1.png" width="50%"/>
<img src="/images/Screenshot_2.png" width="50%"/>
</p>

## Współautorstwo

[jackies-o](https://github.com/jackies-o)

[Michal-Jankowski](https://github.com/Michal-Jankowski)
