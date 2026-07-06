# MovieManager.DAL - Data Access Layer

Questo progetto costituisce il Data Access Layer (DAL) dell'applicazione **MovieManager**. Sviluppato su framework **.NET 10.0**, implementa l'accesso al database tramite **Entity Framework Core**, adottando i pattern **Generic Repository** e **Unit of Work** per centralizzare la logica di persistenza e slegarla dai layer superiori.

---

## 🚀 Prerequisiti e Configurazione

Il progetto richiede il framework target `net10.0` e la presenza del seguente pacchetto NuGet:
* `Microsoft.EntityFrameworkCore`

Le entità di dominio (`Movie`, `Genre`, `Director`, `Actor`, `MovieActor`, `Review`) devono essere già presenti all'interno della cartella `MovieManager.DAL/Entities`.

---

## 🏗️ Architettura del Layer

Il progetto è strutturato nelle seguenti macro-componenti:

### 1. Data Context (`MovieDbContext`)
File: `MovieManager.DAL/Data/MovieDbContext.cs`

Gestisce la connessione e la mappatura ORM delle entità. Include i seguenti `DbSet`:
* `Movies`, `Genres`, `Directors`, `Actors`, `MovieActors`, `Reviews`

> ⚙️ **Configurazioni in OnModelCreating:**
> * Chiave composta su `MovieActor`: `{ MovieId, ActorId }`
> * Relazioni molti-a-uno (`MovieActor -> Movie` e `MovieActor -> Actor`)
> * Campi principali (`Title`, `Name`, `FirstName`, `LastName`, `ReviewerName`) impostati come *Required* e con limiti di lunghezza appropriati.
> * Vincolo di validazione sul punteggio delle recensioni (`Review` score) limitato nel range da 1 a 10.

### 2. Pattern Generic Repository
Consente di astrarre le operazioni CRUD comuni per tutte le entità, evitando la duplicazione del codice.
* **Interfaccia:** `MovieManager.DAL/Repositories/Interfaces/IGenericRepository.cs`
* **Implementazione:** `MovieManager.DAL/Repositories/GenericRepository.cs`

**Metodi inclusi:**
```csharp
Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
Task AddAsync(T entity, CancellationToken cancellationToken = default);
void Update(T entity);
void Remove(T entity);
