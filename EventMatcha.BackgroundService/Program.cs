using Hangfire;
using HangfireBasicAuthenticationFilter;
using EventMatcha.BackgroundServiceCore.Features.Queue;
using FluentValidation;
using EventMatcha.BackgroundService.Services;
using EventMatcha.BackgroundService.Options;
using EventMatcha.BackgroundService.Interfaces;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo;
using Microsoft.Extensions.Options;
using EventMatcha.BackgroundServiceCore.MessageTemplates;
using EventMatcha.BackgroundServiceCore.Features.Email;
using EventMatcha.BackgroundServiceCore.Features.Email.Validators;
using EventMatcha.BackgroundServiceCore.Features.Email.Models;
using EventMatcha.BackgroundServiceCore.Features.SMS.Models;
using EventMatcha.BackgroundServiceCore.Features.Queue.Models;
using EventMatcha.BackgroundServiceCore.Features.SMS.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDbOptions"));
builder.Services.Configure<HangfireOptions>(builder.Configuration.GetSection("HangfireOptions"));
builder.Services.Configure<SMSLive247Options>(builder.Configuration.GetSection("SMSLive247"));
builder.Services.Configure<SMSLive247Options>(builder.Configuration.GetSection("SMSLive247"));


builder.Services.AddQueueServices(builder.Configuration);
builder.Services.AddEmailServices(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<EmailMessageValidator>();
builder.Services.AddScoped<IEmailExecutorService, EmailExecutorService>();
builder.Services.AddScoped<IValidator<EmailMessage>, EmailMessageValidator>();
builder.Services.AddScoped<MessageTemplateServices, MessageTemplateServices>();
builder.Services.AddHttpClient<SmsService>();

builder.Services.AddHangfire((serviceProvider, configuration) =>
{
    var mongoOptions = serviceProvider.GetService<IOptions<MongoDbOptions>>()?.Value;
    var hangfireOptions = serviceProvider.GetService<IOptions<HangfireOptions>>()?.Value;

    if (mongoOptions == null)
    {
        throw new Exception("MongoDbOptions is not configured properly.");
    }

    Console.WriteLine($"ConnectionString: {mongoOptions.ConnectionString}");
    Console.WriteLine($"DatabaseName: {mongoOptions.DatabaseName}");

    var migrationOptions = new MongoMigrationOptions
    {
        MigrationStrategy = new MigrateMongoMigrationStrategy(),
        BackupStrategy = new CollectionMongoBackupStrategy()
    };

    var mongoStorageOptions = new MongoStorageOptions
    {
        MigrationOptions = migrationOptions,
        CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
    };

    configuration.UseMongoStorage(mongoOptions.ConnectionString, mongoOptions.DatabaseName, mongoStorageOptions);
});

builder.Services.AddHangfireServer();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

var hangfireOptions = app.Services.GetService<IOptions<HangfireOptions>>()?.Value;
if (hangfireOptions == null)
{
    throw new Exception("HangfireDashboardOptions is not configured properly.");
}
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "JB Hangfire Job Application",
    DarkModeEnabled = false,
    DisplayStorageConnectionString = false,
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            User = hangfireOptions.Username,
            Pass = hangfireOptions.Password
        }
    }
});

RecurringJob.AddOrUpdate<IEmailExecutorService>(
    "Recurring",
x => x.ProcessQueueMessagesAsync(),
    "*/20 * * * * *");

RecurringJob.AddOrUpdate<IEmailExecutorService>(
    "Recurring",
x => x.ProcessQueueMessagesAsync(),
    "*/20 * * * * *");


    RecurringJob.AddOrUpdate<SmsService>(
    "RecurringSmsJob",
    x => x.SendSmsAsync("+2347039239230", "This is a test SMS message"),
   "*/20 * * * * *");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
