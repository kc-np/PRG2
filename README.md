# PRG2

PRG2 (Programming 2) is a module in semester 1.2 of the CSF course in Ngee Ann Polytechnic, focusing on C# programming.

This repository contains my C# code written for the module's assignment.

This program utilizes Object Oriented Programming to develop a simple Movie Ticketing System. 

Background information:

Singa Cineplexes, a leading cinema exhibitor on our island country state, has engaged your team to develop a movie ticketing system to computerize the movie ticket sales process. For a start, its management has requested for a simple prototype of the system to better establish the effectiveness of this solution as a system for ticket counter staff and as a self-service kiosk.

A list of cinema halls and movie information is centrally managed by headquarters. Staff can then schedule and manage a movie screening session based on the given details i.e., Cinema Hall and Movie. Each screening of a movie can either be in a 2D or 3D format. A 30 minutes cleaning time is allocated after the end of every movie screening session.

During the ticket purchase process, the counter staff will require the purchaser to provide the age of the ticket holder should fall under the PG13, NC16, M18, or R21 categories*. The ticket will only be sold if the age requirement is met.

To support price discrimination strategies**, tickets are sold at different prices depending on several factors such as days of the week, type of screening (e.g., 3D, 2D), opening date of the movie, and selected concession holders.

If a customer wishes to purchase a student ticket, the level of study must be provided (e.g., Primary, Secondary, Tertiary). For a senior citizen ticket, the ticket holder’s age must be 55 and above. By default, an adult ticket is to be purchased, even for children who do not fall into the student category. For customers buying adult tickets, they will also be entitled to purchase a popcorn set at a discounted rate of $3.00.


*Categories:

G (General): Suitable for all ages

PG13 (Parental Guidance 13): Restricted to persons aged 13 and above

NC16 (No Children Under 16): Restricted to persons aged 16 and above

M18 (Mature 18): Restricted to persons aged 18 and above

R21 (Restricted 21): Restricted to persons aged 21 and above


**Price discrimination strategies:

<table>
    <thead>
        <tr>
            <th rowspan="2"></th>
            <th colspan="4">Screening Type</th>
        </tr>
        <tr>
            <th colspan="2">3D Movies</th>
            <th colspan="2">2D Movies</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Days of Week</td>
            <td>Monday to Thursday</td>
            <td>Friday to Sunday</td>
            <td>Monday to Thursday</td>
            <td>Friday to Sunday</td>
        </tr>
        <tr>
            <td>Adult Price</td>
            <td>$11.00</td>
            <td rowspan="3">$14.00</td>
            <td>$8.50</td>
            <td rowspan="3">$12.50</td>
        </tr>
        <tr>
            <td>Student Price</td>
            <td>$8.00</td>
            <td>$7.00</td>
        </tr>
        <tr>
            <td>Senior Citizen Price</td>
            <td>$6.00</td>
            <td>$5.00</td>
        </tr>
        <tr>
            <td>First 7 days of movie opening date</td>
            <td colspan="4">Student and senior citizen tickets are to be charged at Adult ticket price</td>
        </tr>
    </tbody>
</table>