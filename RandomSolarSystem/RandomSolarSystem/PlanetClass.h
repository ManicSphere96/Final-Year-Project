#pragma once
#include <vector>
#include <string>
class CelestialBody 
{
public:

	
	CelestialBody(int i) { m_PlanetNumber = i; }
	CelestialBody() {};
	int GetNumber() { return m_PlanetNumber; }
	std::string GetStatus() { return m_Status; }
	void SetStatus(std::string status) { m_Status = status; }
	double GetDistance() { return m_Distance; }
	void SetDistance(double distance) { m_Distance = distance; }
	void AddDistance(double distance) { m_Distance += distance; }
	double GetInitVelocity() { return m_InitVel; }
	void SetInitVel(double newvel) { m_InitVel = newvel; }
	void AddInitVel(double newvel) {m_InitVel = newvel + m_InitVel;}
	double GetMass() { return m_Mass; }
	void SetMass(double mass) { m_Mass = mass; }
	void AddMass(double mass) { m_Mass += mass; }
	std::vector<float> GetAngle () { return m_Angle; }
	void SetAngle(std::vector<float> angle) { m_Angle = angle; }
	void AddAngle(std::vector<float> angle){ for (int i = 0; i < m_Angle.size(); i++) { m_Angle[i] = angle[i] + m_Angle[i]; } }
	double GetRev() { return m_RevPerHour; }
	void SetRev(double rev) { m_RevPerHour = rev; }
	void AddRev(double rev) { m_RevPerHour += rev; }
	float GetOI() { return M_OrbitalInclination; }
	void SetOI(float OI) { M_OrbitalInclination = OI; }
	void AddOI(float OI) { M_OrbitalInclination += OI; }



private:
	int m_PlanetNumber;
	std::string m_Status;
	double m_Distance;
	double m_InitVel;
	double m_Mass;
	std::vector<float> m_Angle;
	double m_RevPerHour;
	float M_OrbitalInclination;
};
