# 🧰 JSON to CSV Converter

A simple and efficient tool built with **.NET 6** to convert **JSON** files into **CSV**, supporting nested structures and arrays.

> Perfect for developers or data analysts who need to quickly transform API responses into spreadsheets for reports or analysis.

---

## 📦 Features

- 📄 Handles complex, deeply nested JSON structures
- 📊 Generates dynamic CSV columns sorted alphabetically
- 🧹 Prevents formatting issues when opening in Excel
- 🔐 Includes error handling for reading, parsing, and writing files
- 🚀 Automatically opens the generated CSV (when supported by the OS)

---

## 🧑‍💻 How to Use

### 1. Build the project

```bash
dotnet build
```

### 2. Run with the path to your JSON file

```bash
dotnet run -- "C:\path\to\your\file.json"
```

> The output CSV file will be created in the same folder with `_out.csv` appended to the filename.

---

## 💡 Example

Given a `data.json` file with the following content:

```json
[
  {
    "id": 1,
    "name": "John",
    "contact": {
      "email": "john@email.com",
      "phones": ["1111-1111", "2222-2222"]
    }
  }
]
```

The generated CSV will look like this:

```csv
contact.email;contact.phones[0];contact.phones[1];id;name
"john@email.com";"1111-1111";"2222-2222";"1";"John"
```

---


## 🧠 Code Structure

- `Main`: Handles file validation, JSON parsing, CSV generation, and file output.
- `FlattenJson`: A recursive function that flattens nested JSON objects and arrays into key-value pairs suitable for CSV.

---
