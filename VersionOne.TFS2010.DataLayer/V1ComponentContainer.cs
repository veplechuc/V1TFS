using System;
using VersionOne.ServiceHost.Core.Configuration;
using log4net;

namespace VersionOne.TFS2010.DataLayer 
{
    // TODO should it be thread safe?
    public class V1ComponentContainer 
    {
        private readonly ILog logger = LogManager.GetLogger(typeof (V1ComponentContainer));

        private V1Component component;
        
        public V1Component GetV1Component(VersionOneSettings settings) 
        {
            if(component == null || DifferentSettingsRequired(settings)) 
            {
                component = new V1Component(settings);

                if(!component.ValidateConnection()) 
                {
                    logger.Info("Failed to validate VersionOne connection");
                    throw new InvalidOperationException("Failed to validate VersionOne connection");
                }
            
                logger.Info("Successfully connected to VersionOne.");
            }

            return component;
        }

        private bool DifferentSettingsRequired(VersionOneSettings settings) 
        {
            return component == null || !component.Settings.Equals(settings);
        }
    }
}