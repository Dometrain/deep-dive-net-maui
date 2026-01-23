using Polly;
using Polly.Retry;
using SQLite;

namespace HelloMaui.Database;

public abstract class BaseDatabase
{
	readonly Lazy<SQLiteAsyncConnection> _sqliteDatabaseHolder;

	protected BaseDatabase(IFileSystem fileSystem)
	{
		var databasePath = Path.Combine(fileSystem.AppDataDirectory, "SqliteDatabase.db3");
		_sqliteDatabaseHolder = new(() => new SQLiteAsyncConnection(databasePath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache));
	}

	SQLiteAsyncConnection DatabaseConnection => _sqliteDatabaseHolder.Value;

	protected async Task<TReturn> Execute<TReturn, TDatabase>(Func<SQLiteAsyncConnection, Task<TReturn>> action, CancellationToken token, int maxRetries = 10)
	{
		var databaseConnection = await GetDatabaseConnection<TDatabase>().ConfigureAwait(false);

		var resiliencePipeline = new ResiliencePipelineBuilder<TReturn>()
			.AddRetry(new RetryStrategyOptions<TReturn>
			{
				MaxRetryAttempts = maxRetries,
				Delay = TimeSpan.FromMilliseconds(2),
				BackoffType = DelayBackoffType.Exponential
			}).Build();

		return await resiliencePipeline.ExecuteAsync(async _ => await action(databaseConnection), token);
	}

	async ValueTask<SQLiteAsyncConnection> GetDatabaseConnection<T>()
	{
		if (DatabaseConnection.TableMappings.All(static x => x.MappedType == typeof(T)))
		{
			await DatabaseConnection.EnableWriteAheadLoggingAsync().ConfigureAwait(false);
			await DatabaseConnection.CreateTableAsync(typeof(T)).ConfigureAwait(false);
		}

		return DatabaseConnection;
	}
}