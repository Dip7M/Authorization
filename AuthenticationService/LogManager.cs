using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace AuthenticationService
{
    public interface ILoggerManager
    {
        void Info(string message);
        void Error(string message);
        void Warn(string message);
    }
    public class LoggerManager: ILoggerManager
    {
        //private readonly ILog _logger = LogManager.GetLogger(typeof(LoggerManager));
        private readonly ILog _logger = LogManager.GetLogger(typeof(LoggerManager));

        public LoggerManager()
        {
            try
            {
                XmlDocument log4netConfig = new XmlDocument();

                using (var fs = File.OpenRead("log4net.config"))
                {
                    log4netConfig.Load(fs);
                    var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
                    XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
                    _logger.Info("Log System Initialized");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error", ex);
            }
        }
        public void Info(string message)
        {
            _logger.Info(message);
        }
        public void Error(string message)
        {
            _logger.Error(message);
        }
        public void Warn(string message)
        {
            _logger.Warn(message);
        }
    }
}
