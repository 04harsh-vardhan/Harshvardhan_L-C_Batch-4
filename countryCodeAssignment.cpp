#include <iostream>
#include <string>
#include <map>

std::string getCountryName(std::string countryCode)
{
    std::map<std::string, std::string> countryCodeWithName = {
        {"IN", "India"},
        {"AUS", "Australia"},
        {"NZ", "NewZealand"},
        {"US", "United states"}};
    auto countryIterator = countryCodeWithName.find(countryCode);
    if(countryIterator != countryCodeWithName.end())
    {
        return countryIterator->second;
    }
    else
    {
        return "No country exists with that code\n try again\n";
    }
}

int main()
{
    while(1)
    {
    std::cout << "Type the country code" << std::endl;
    std::string countryCode;
    std::cin >> countryCode;
    std::cout<<getCountryName(countryCode)<<std::endl;
    }
}