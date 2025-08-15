# Contact Lookup App

A simple C# console application for managing contacts.  
Users can add, update, look up, and delete contacts, with data stored in a JSON file for persistence.

---

## Features

- **Lookup Contact**: Case-insensitive search by name using LINQ.
- **Add Contact**: Adds a contact with name and phone validation.
- **Update Contact**: Updates an existing contact’s phone number.
- **Delete Contact**: Removes a contact by phone number.
- **Persistent Storage**: Contacts are saved to and loaded from `Data/contacts.json`.

---

## Validation Rules

- **Name**
  - Minimum length: 2 characters.
  - Allowed: letters, spaces, apostrophes (`'`), hyphens (`-`).
  - Multiple spaces are collapsed into one.
  - Duplicate names (case-insensitive) are not allowed.

- **Phone**
  - Must contain exactly 10 digits total.
  - Non-digit formatting (spaces, dashes, parentheses) is accepted on input.
  - Saved format: `###-###-####`.
  - Duplicates are detected by digits only (format-insensitive).

---

## Quick Start

### Prerequisites
- .NET SDK (8.0+ recommended)

### Build and Run
```bash
dotnet build
dotnet run --project path/to/your/ContactLookup.csproj
```

On first run, the app seeds a small set of contacts and writes them to `Data/contacts.json`.  
Subsequent runs load the existing file so your changes persist.

---

## Example Usage

```
------------------
1. Lookup Contact
2. Add Contact
3. Update Contact
4. Delete Contact
5. Exit
------------------
(Choose an option)
```

- Follow the prompts to enter names or phone numbers.
- Lookup is case-insensitive.
- Phone numbers can be typed with or without formatting; validation will normalize them.

---

## Data Storage

- File path: `Data/contacts.json` (relative to the executable).
- First run: seeds default contacts and creates the file.
- Later runs: reads existing data and writes changes on add/update/delete.

---

## Project Structure (example)

```
ContactLookup/
├─ src/
│  └─ ContactLookup/
│     ├─ Program.cs
│     └─ ContactLookup.csproj
├─ Data/
│  └─ contacts.json           # created at runtime
├─ README.md
├─ .gitignore
├─ .editorconfig
└─ .gitattributes
```

*(Your structure may vary. The `Data` folder is created automatically if missing.)*

---

## Implementation Notes

- **LINQ**:
  - `Where` + `OrderBy` for filtering/sorting.
  - Case-insensitive comparisons via `StringComparison.OrdinalIgnoreCase`.
- **Deferred vs Materialized**:
  - Lookup returns `IEnumerable<Contact>` for flexibility; materialize with `.ToList()` when needed.
- **Phone Normalization**:
  - Digits are extracted with `char.IsDigit`.
  - Comparisons use digits-only to avoid format mismatches.
- **Persistence**:
  - `System.Text.Json` for serialization/deserialization.
  - Safe default with `Deserialize(... ) ?? new List<Contact>()`.
  - Basic error handling recommended around file I/O.

---

## Roadmap / Future Improvements

- Email capture and validation.
- Search by phone digits (substring match).
- “List All” command (sorted by name).
- Unit tests (xUnit) for validators and helpers.
- Export/Import (CSV/JSON).
- Better error handling and logging.
- Optional: persistence to a database (SQLite + EF Core) or a simple REST API.

---

## License

MIT License (or your preferred license). Add a `LICENSE` file at the repo root.
