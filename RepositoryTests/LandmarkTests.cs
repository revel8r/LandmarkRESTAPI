using Microsoft.VisualStudio.TestTools.UnitTesting;
using Revel8R.Landmark.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revel8R.Landmark.BusinessEntities;
using Revel8R.BusinessEntities;

namespace Landmark.Repository.Tests
{
    [TestClass()]
    public class LandmarkTests
    {
        private LandmarkRepository repository;

        public void Initialize()
        {
            if (this.repository == null)
            {
                this.repository = new LandmarkRepository();
            }
        }

        [TestMethod()]
        public void LandmarkTestReadAllUSVirginia()
        {
            this.Initialize();
            LandmarkKey key = new LandmarkKey();
            key.SearchTerms = new Dictionary<string, string>();
            key.SearchTerms.Add("country", "USA");
            key.SearchTerms.Add("stateProvince", "Virginia");
            BaseEntityCollection<LandmarkEntity> virginiaLandmarks = this.repository.ReadAll(key);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LandmarkTestReadAllNoKey()
        {
            this.Initialize();
            this.repository.ReadAll(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LandmarkTestReadAllNoSearchTerms()
        {
            this.Initialize();
            LandmarkKey key = new LandmarkKey();
            this.repository.ReadAll(key);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LandmarkTestReadAllCountryOnly()
        {
            this.Initialize();
            LandmarkKey key = new LandmarkKey();
            key.SearchTerms = new Dictionary<string, string>();
            key.SearchTerms.Add("country", "USA");
            this.repository.ReadAll(key);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LandmarkTestReadAllStateOnly()
        {
            this.Initialize();
            LandmarkKey key = new LandmarkKey();
            key.SearchTerms = new Dictionary<string, string>();
            key.SearchTerms.Add("stateProvince", "Virginia");
            this.repository.ReadAll(key);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LandmarkTestReadAllNoCountryNoState()
        {
            this.Initialize();
            LandmarkKey key = new LandmarkKey();
            key.SearchTerms = new Dictionary<string, string>();
            key.SearchTerms.Add("country", string.Empty);
            key.SearchTerms.Add("stateProvince", string.Empty);
            this.repository.ReadAll(key);
        }
    }
}