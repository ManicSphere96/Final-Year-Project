#include <iostream>
#include <cstdlib>
#include <random>
#include <math.h>

#include "NormInv.h"
#include "PlanetClass.h"


double StableVelocity(double SunMass, double Distance)
{
    // this gives avelocity to obtain a circular orbit for the object in question. 
    return (double)sqrt((6.6743e-11f * SunMass) / Distance);
}

int main()
{
    bool SystemActive = true;
    bool IntegerGiven = false;
    int NumberOfPlanets;
    int NumOfRockPlanets;
    int NumOfGasPlanets;
    std::vector <CelestialBody> Planets;
    double MinRock = 3.3e23;
    double MaxRock = 15e24;
    double MinGas = 15e24;
    double MaxGas = 2467e24;
    double mu = 89.18;
    double sigma = 0.89377;
    std::random_device RandomDevice;
    while (SystemActive)
    {
        IntegerGiven = false;
        
        std::cout << "Welcome to the solar system generator.\n \n";
        while (!IntegerGiven)
        {
            NumberOfPlanets = 0;
            std::cout << "Please enter number of planets in your random solar system between 2 and 12.\n";
            std::cin >> NumberOfPlanets;

            for (int i = 2; i < 13; i++)
            {
                if (NumberOfPlanets == i)
                {
                    IntegerGiven = true;
                }
            }
            
            std::cin.clear();
            std::cin.ignore(256, '\n');
        }
        CelestialBody Sun = CelestialBody();// initializing a star 
        Sun.SetStatus("Sun");
        // from data given by our Solar system we can estimate that rocky planets and gas giants are equally likely
        //thus half the planets will be Terrestrial(rocky) Planets and th other half Gas giants.
        if (NumberOfPlanets % 2 != 0)
        {
            NumberOfPlanets--;
            NumOfRockPlanets = NumberOfPlanets / 2;
            NumOfGasPlanets = NumOfRockPlanets;
            NumOfRockPlanets++;
            NumberOfPlanets++;
        }
        else
        {
            NumOfRockPlanets = NumberOfPlanets / 2;
            NumOfGasPlanets = NumOfRockPlanets;
        }

        for (int i = 0; i < NumberOfPlanets; i++)
        {
            Planets.push_back(CelestialBody(i)); //generating the list of planets
            if (i < NumOfRockPlanets)
            {
                Planets[i].SetStatus("Rock");
            }
            else
            {
                Planets[i].SetStatus("Gas");
            }
            
        }

        std::default_random_engine RandomEngine(RandomDevice());
        std::uniform_real_distribution<double> sununif(0.57, 1.64);
        Sun.SetMass(sununif(RandomEngine)); //Giving our sun object a Mass

        for (int i = 0; i < Planets.size(); i++)
        {
            if (Planets[i].GetStatus() == "Rock")
            {
                std::uniform_real_distribution<double> unif(MinRock, MaxRock);
                Planets[i].SetMass(unif(RandomEngine)); 
                //Setting a random mass Based off data minimum based off Mercury Max Based on point at which rocky planet becomes a gas giant.
            }
            else if (Planets[i].GetStatus() == "Gas")
            {
                std::uniform_real_distribution<double> unif(MinGas, MaxGas);
                Planets[i].SetMass(unif(RandomEngine));
                // Setting a random mass Based off data minimum based off Max above and Point at which Gas giants can become Brown dwarf stars
            }
            std::uniform_real_distribution<double> Chanceunif(0.0, 100.0);
            if (Chanceunif(RandomEngine) < 90.0) // 90%
            {
                std::uniform_real_distribution<double> unif(0, 1.0);
                Planets[i].SetOI(norminv(unif(RandomEngine), mu, sigma));
                //creating an orbital inclination based off the normal distribution of the data given by exoplanet.eu_catalog
            }
            else// 10%
            {
                std::uniform_real_distribution<double> unif(0.0,180.0);
                Planets[i].SetOI(unif(RandomEngine));
                //roughly 10 percent of data in exoplanet.eu_catalog suggested that the Orbital inclination was random.
            }

            // For distance all ecamples of solar systems larger than 5 planets followed an exponential curve almost exactly 
            std::uniform_real_distribution<double> Distunif(0.008, 0.2);
            Planets[i].SetDistance((Distunif(RandomEngine) * exp(0.7 * (i + (int)1)))); // this models the exponential curve.

            Planets[i].SetInitVel(StableVelocity((Sun.GetMass()* 1.989e30f),Planets[i].GetDistance()));
            

           

           // std::cout << "Planet # " << Planets[i].GetNumber() << " Planet Mass " << Planets[i].GetMass() << " Planet orbital inclination " << Planets[i].GetOI() << " Planet Distance from sun " << Planets[i].GetDistance()<<" The planets orbital velocity "<< Planets[i].GetInitVelocity() <<std::endl;
        }
        std::cout << "Planet # " << "  Mass " << " orbital inclination " << " Distance from sun " << " orbital velocity "<< std::endl;
        for (int i = 0; i < Planets.size(); i++)
        {
            std::cout << Planets[i].GetNumber()<< "     "<< Planets[i].GetMass() << "      " << Planets[i].GetOI() << "        " << Planets[i].GetDistance() << "            " << Planets[i].GetInitVelocity() << std::endl;
        }
    }
}



