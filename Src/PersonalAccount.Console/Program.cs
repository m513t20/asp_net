using PersonalAccount.Domain;

Console.WriteLine(LogoConf.GetLogo());

while (true)
{
    await Task.Delay(TimeSpan.FromHours(1));
}