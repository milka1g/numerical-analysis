import sympy as sp
import math as m
from fractions import Fraction
from decimal import *
from math import log10, floor

def pretty_print_fractions(poly):
    coeffs = poly.all_coeffs()
    maxcoef = len(coeffs) - 1
    frac_coeffs = []
    for coeff in coeffs:
        frac_coeffs.append(Fraction(str(coeff)).limit_denominator())
    for i in range(0, len(frac_coeffs)):
        if(frac_coeffs[i].numerator == 0):
            maxcoef = maxcoef - 1
            continue
        sign = '+' if frac_coeffs[i].numerator > 0 else ''
        print(f"{sign if i>0 else ''}{frac_coeffs[i].numerator}/{frac_coeffs[i].denominator}*x^{maxcoef}", end='')
        maxcoef = maxcoef - 1
    print('')

def pretty_print_power10(poly):
    global k
    coeffs = poly.all_coeffs()
    maxcoef = len(coeffs) - 1
    power10 = []
    multiplied_coeffs = []
    for coeff in coeffs:
        mul = find_exp(abs(coeff)) if coeff != 0 else -1
        power10.append(mul)
        multiplied_coeffs.append(coeff*10**abs(mul))
    for i in range(0, len(power10)):
        if(power10[i] == -1): #there is no coeff
            maxcoef = maxcoef - 1
            continue
        sign = '+' if coeffs[i] > 0 else ''
        print(f"{sign if i>0 else ''}", f"{{:.{k+1}}}".format(multiplied_coeffs[i]), f"*10^{power10[i]}*x^{maxcoef}", end='')
        maxcoef = maxcoef - 1
    print('')

def k_incr(num):
    ret = 0
    for ch in str(num).split(".")[1]:
        if ch == '0':
            ret = ret + 1
        else:
            break
    return ret + 1

def find_exp(number):
    base10 = log10(number)
    return floor(base10)

def count_sign_changes(v_arr):
    changes = 0
    v_arr = [v for v in v_arr if v != 0]
    for i in range(1,len(v_arr)):
        if v_arr[i]<0 and v_arr[i-1]>0 or v_arr[i]>0 and v_arr[i-1]<0:
            changes = changes + 1
    return changes

x = sp.symbols('x')
P = sp.poly(m.e*x**3-m.pi**2*x**2+2*m.pi*x+4)
#P = sp.poly((m.pi/1260-1/420)*x**8 + (-m.pi**2/1680+m.pi/840)*x**7 + (-m.pi/30 + 1/10)*x**6 + (-m.pi**2/60 + m.pi/30)*x**5 + (2*m.pi/3 - 2)*x**4)
#P = sp.poly(x**9 - 3*x**7 - x**6 + 3*x**5 + 3*x**4 - x**3 - 3*x**2 + 1)
print("Starting poly:", str(P))

if str(P.domain) == "RR":
    k = int(input("Insert k (decimal places): "))
    coeffs = P.all_coeffs()
    for i in range(0, len(coeffs)):
        if coeffs[i] > 0:
            with localcontext() as ctx:
                ctx.rounding = ROUND_DOWN
                coeffs[i] = float(round(Decimal(str(coeffs[i])), k + k_incr(coeffs[i])))
        elif coeffs[i] < 0:
            with localcontext() as ctx:
                ctx.rounding = ROUND_UP
                coeffs[i] = float(round(Decimal(str(coeffs[i])), k + k_incr(coeffs[i])))
    P = sp.Poly(coeffs, x)
    print("Rounded P:", P)
    print("Powers of 10: ", end='')
    pretty_print_power10(P)
    print("Fractions P: ", end='')
    pretty_print_fractions(P)

Pdiff = sp.diff(P)
print("Pdiff:", Pdiff)
G = sp.gcd(P, Pdiff)
print("GCD:", G)

P0 = sp.div(P, G)[0]
P1 = sp.diff(P0)
print("P0 =", P0)
print("P1 =", P1)

P_arr = [P0, P1]
while sp.degree(P0) != 1:
    Q = -sp.rem(P0, P1)
    print("P" + str(len(P_arr)) + " = " + str(Q))
    P_arr.append(Q)
    P0 = P1
    P1 = Q

a = input("Insert a [a,b]: ")
b = input("Insert b [a,b]: ")

V_a = []
V_b = []
for p in P_arr:
    V_a.append(p.subs(x, a))
    V_b.append(p.subs(x, b))
print("V_a: ", V_a)
print("V_b: ", V_b)
print("Sign changes in V_a:", count_sign_changes(V_a))
print("Sign changes in V_b:", count_sign_changes(V_b))
print(f"Broj nula polinoma P(x) = {str(P)} na segmentu:[{a},{b}]:", count_sign_changes(V_a) - count_sign_changes(V_b))






