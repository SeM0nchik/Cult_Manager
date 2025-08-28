# Cultist Simulator: Cult Management System (Variant #3)

## Project Overview

This is a console application for managing cult data from "Cultist Simulator", built in C# (.NET 8.0). It features a fully custom JSON parser and provides tools for loading, filtering, sorting, modifying, and saving cult information. Developed as an educational project with permission from Weather Factory.

## Project Structure

The solution is organized into five core projects, ensuring a clean separation of concerns:

```
Project_3_Advanced.sln
├── JsonObject/                 # Data Models & Contracts
│   ├── IJSONObject.cs          // Interface for all JSON-serializable objects
│   └── Cult.cs                 // The Cult model class
├── StateMachine/               # Parsing Logic Core
│   └── JsonStateMachine.cs     // The core recursive descent parser logic
├── Parser/                     # Parser Interface
│   └── JsonParser.cs           // Static class bridging the parser to the app
├── MenuLibrary/                # User Interface
│   └── ConsoleMenu.cs          // Handles menu rendering and navigation
└── Project_Main/               # Application Core
    ├── Program.cs              // Main application logic and workflow
    ├── DataManager.cs          // Handles in-memory data operations
    └── FileService.cs          // Manages file I/O operations
```

### Data Locations:
-   `WorkingFiles/` - Primary directory for JSON files
-   `data/` - Additional data samples and schemas

## The Parser: Technical Heart

The most innovative part is the custom JSON parser in the `StateMachine` project. It uses **two interdependent static state machines** working through **recursive descent** to parse complex nested JSON structures without any external libraries.

**How it works:**
1.  **Tokenizer Machine:** Scans the input character-by-character, identifying tokens (`{`, `}`, `:`, `strings`, `numbers`).
2.  **Parser Machine:** Processes the token stream, using recursive calls to handle nested objects/arrays.
3.  The entire system is abstracted through the `JsonParser` static class in the Parser project.

## Main Menu & Navigation

The application features this main menu interface:

```
[1] Ввести данные
[2] Фильтровать данные
[3] Сортировать данные
[4] <основная задача> изменить данные
[5] Вывести данные
[6] Выход
```

**Navigation:** Enter the number of your desired option and press Enter.

## Complete Functionality Guide

### 1. [1] Ввести данные (Input Data)
-   **Source Selection:** Choose between console input or file path
-   **File Input:** Provide path to JSON file (`WorkingFiles/cults.json`) or find appropriate file in directory.
-   **Console Input:** Paste JSON data directly
-   The custom parser processes input into Cult objects

### 2. [2] Фильтровать данные (Filter Data)
-   Select from available fields (id, label, aspects and etc)
-   Enter data (e.g., id -> "12314124"). You can enter a list of data
-   View filtered results while maintaining all field visibility

### 3. [3] Сортировать данные (Sort Data)
-   Choose field for sorting (string/numeric fields only)
-   Select ascending/descending order
-   Collections are sorted in-place

### 4. [4] Изменить данные (Modify Data) - Core Feature
**Complete CRUD operations for cult management:**
-   **Add New Cults:** Create new cult entries with unique IDs
-   **Edit Existing:** Modify any field of existing cults by ID
-   **Delete Cults:** Remove cults from the collection
-   **Merge Files:** Combine multiple JSON files with conflict resolution
-   **Data Validation:** Ensures data integrity throughout operations

### 5. [5] Вывести данные (Output Data)
-   **Console Output:** Displays formatted data in console
-   **File Export:** Saves data to JSON file (preserves structure)
-   **Auto-encoding:** Handles Russian text and special characters

### 6. [6] Выход (Exit)
-   Clean application termination
-   Confirmation prompt for unsaved changes

## Key Features

-   **Custom JSON Parser:** Handles formatted/minified JSON without external libraries
-   **Error Resilience:** Graceful handling of invalid paths, malformed JSON, and user errors
-   **Unicode Support:** Full Russian language support in input/output
-   **Relative Paths:** Files work across different machines and locations
-   **Data Integrity:** Default values for missing fields, consistent JSON structure

## How to Use

1.  Place your JSON files in `WorkingFiles/` directory
2.  Run the application
3.  Use option `[1]` to load data from file:
    ```
    Enter path: WorkingFiles/cults.json
    ```
4.  Use options `[2]` and `[3]` to refine data
5.  Use option `[4]` for advanced data modification
6.  Use option `[5]` to save results:
    ```
    Enter output path: WorkingFiles/updated_cults.json
    ```

## Technical Excellence

-   **SOLID Compliance:** Clean separation through 5 specialized projects
-   **Recursive Parsing:** Advanced state machine implementation
-   **Full Documentation:** Comprehensive code comments throughout
-   **Memory Safe:** Proper disposal of unmanaged resources
-   **Cross-Platform:** Compatible with any .NET 8.0 environment

---

**Developer:** [Pavlichev Semyen]  
**Variant:** #3 (CS: Cults)  
**Status:** Fully Implemented with Custom Parser
