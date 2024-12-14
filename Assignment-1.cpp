#include <iostream>
#include <string>
#include <map>

std::string getCountryName(std::string countryCode)
{
    std::map<std::string, std::string> countryCodewithCountryName = {
        {"IN", "India"},
        {"AUS", "Australia"},
        {"NZ", "NewZealand"},
        {"US", "United states"}};
    auto countryIterator = countryCodewithCountryName.find(countryCode);
    if(countryIterator != countryCodewithCountryName.end())
    {
        return countryIterator->second;
    }
    else
    {
        return "No Country Exists with That Code";
    }
}

int main()
{
    while(1)
    {
    std::cout << "Type the country Code" << std::endl;
    std::string countryCode;
    std::cin >> countryCode;
    std::cout<<getCountryName(countryCode)<<std::endl;
    }
}