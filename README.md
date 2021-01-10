# SWE1-MTCG
Moster Trading Card Game - Software Engineering 1

git-link: https://github.com/schlechr/SWE1-MTCG.git

Basic Flow:
First i used the REST Server from the first SWE project as base.
The first big steps where to create and build a connection to a Postgres DB and the converting of the sent content as JSON to useful data. 
After those steps where working correctly (i tested and played with the functions befor starting editing the real project), i started with registartion of a User. 
With the registration of a User it was also time to think about a Database structure which should handle the most of the requirements. Of course i thaught that i will not get all of the tables and columns i need from scratch but the plan was to get a basic structure from begin on. 
To get a better view for all of the different functionalitys i sorted the CRUD Requests into different classes and worked from there on on each functionality with classes seperated to the existing tables in the database.
After that it was just developing each function on it's own and look to use the classes as cross-functional as possible.
At the end of the project i came to a issue where there were too much connection open to the DB and the process crashed. As it said in the error, the connections were not closed correctly, as i opend one at each new function. Before i started to create a own class for DB requests, just to have a better code quality, this helped me to solve this issue by adding the connection itself to this class as well and writting a deconstructor, which is closing the connection.


Time Spent:
1 .Jan: 3h
2 .Jan: 4h
3 .Jan: 4h
4 .Jan: 8h
8 .Jan: 14h
9 .Jan: 10h
10.Jan: 8hs

@all: 51h