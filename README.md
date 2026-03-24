# Equipment Rental System (APBD)

Prosta aplikacja konsolowa w C# do obsługi wypożyczalni sprzętu na uczelni.

# Uruchomienie

1. Zainstaluj .NET 8 SDK
2. Wejdź do folderu: EquipmentRental
3. Uruchom: dotnet run

# Funkcje

- dodawanie użytkowników (Student, Employee)
- dodawanie sprzętu (Laptop, Projector, Camera)
- lista całego sprzętu i lista dostępnego sprzętu
- wypożyczanie z kontrolą limitów i dostępności
- zwroty z karą za spóźnienie (10 PLN / dzień)
- oznaczanie sprzętu jako uszkodzony
- podgląd aktywnych wypożyczeń użytkownika
- lista przeterminowanych wypożyczeń
- raport podsumowujący stan wypożyczalni
- eksport danych do JSON

# Jak podzieliłem projekt i dlaczego

Kod podzieliłem na trzy części, które się między sobą komunikują:

- Program.cs - odpalenie programu i obsługa menu konsolowego
- Services - logika biznesowa w RentalService (wypożyczenia, zwroty, limity, kary, raport, eksport)
- Models - obiekty domenowe (Equipment, User, Rental i klasy, które je dziedziczą)

# Kohezja, coupling i odpowiedzialność klas

- Kohezja: klasy modeli trzymają głównie dane, a RentalService skupia się na operacjach wypożyczalni.
- Coupling: Program.cs wywołuje metody serwisu, ale nie implementuje reguł 
biznesowych jak limity klientów, więc wszystkie funkcje wiedzą tylko to co jest im niezbędne.
- Odpowiedzialność klas:
  - Student i Employee mają ustawione limity wypożyczeń
  - RentalService pilnuje zasad wypożyczeń i zwrotów
  - Program.cs odpowiada za interakcję z użytkownikiem

# Uwagi końcowe

Projekt był rozwijany etapami (kolejne commity i gałęzie robocze "full-menu" oraz "readme"). Finalna wersja znajduje się w gałęzi main i zawiera pełne menu oraz dodatkową funkcję eksportu danych do JSON.