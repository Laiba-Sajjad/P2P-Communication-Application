#include<iostream>
#include<string>
#include"Account.h"
using namespace std;

void customer::set(string n, string add, string c, string ph)
{
	name = n;
	address = add;
	cnic = c;
	phone = ph;

}
void customer::get()
{
	cout << cnic << "<<" << name << "<<" << phone << "<<" << address << endl;
}
void Account::setacc(string acc, string accdate){

	accountnum = acc;
	accountdate = accdate;
}
void Account::getacc(){

	cout << accountnum << balance << accountdate;
	customer::get();
}

void Account::depositmoney(){
	cout << "Enter amount to be deposited: ";
	cin >> addbal;
	balance = balance + addbal;
}
int Account::withdraw(){
	cout << "Enter amount you want to withdraw: ";
	cin >> rembal;
	balance = balance - rembal;
	return balance;
}
void Account::getbalance(){
	cout << balance << endl;
}

void savingaccount::changeinterest(float cinterest){
	interestrate = cinterest;
}
int savingaccount::setbalance(int nofm){
	balance = addbal + ((interestrate / 100)*addbal*nofm);
	return balance;
}
void checkingaccount::changeiFF(float iff){
	insufficientFundFee = iff;
}
void checkingaccount::getiffbalance(){
	if (balance < 10000){
		balance = addbal - insufficientFundFee;
	}
	else
		balance = addbal;
}
float Account::balance = 0;