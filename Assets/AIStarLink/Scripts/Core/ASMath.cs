using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASMath : MonoBehaviour
{

    public void Start()
    {
        DontDestroyOnLoad(this);
    }

    private const int NTAB = 32;
    private const int IA = 16807;
    private const int IM = 2147483647;
    private const int IQ = 127773;
    private const int IR = 2836;
    private const int NDIV = (1 + (IM - 1) / NTAB);
    private const ulong MAX_RANDOM_RANGE = 0x7FFFFFFF;
    private const float AM = 1.0f / IM;
    private const float EPS = 1.2f / 1000 / 10000;
    private const float RNMX = (1.0f - EPS);

    private int m_idum = 0;
    private int m_iy = 0;
    private int[] m_iv = new int[NTAB];

    public void SetSeed(int iSeed)
    {
        m_idum = ((iSeed < 0) ? iSeed : -iSeed);
        m_iy = 0;
    }

    public int GenerateRandomNumber()
    {
        int j;
        int k;

        if (m_idum <= 0 || m_iy == 0)
        {
            if (-(m_idum) < 1)
                m_idum = 1;
            else
                m_idum = -(m_idum);

            for (j = NTAB + 7; j >= 0; j--)
            {
                k = (m_idum) / IQ;
                m_idum = IA * (m_idum - k * IQ) - IR * k;
                if (m_idum < 0)
                    m_idum += IM;
                if (j < NTAB)
                    m_iv[j] = m_idum;
            }
            m_iy = m_iv[0];
        }
        k = (m_idum) / IQ;
        m_idum = IA * (m_idum - k * IQ) - IR * k;
        if (m_idum < 0)
            m_idum += IM;
        j = m_iy / NDIV;

        // We're seeing some strange memory corruption in the contents of s_pUniformStream. 
        // Perhaps it's being caused by something writing past the end of this array? 
        // Bounds-check in release to see if that's the case.
        if (j >= NTAB || j < 0)
        {
            UnityEngine.Debug.LogFormat("CUniformRandomStream had an array overrun: tried to write to element {0} of 0..31. Contact Tom or Elan.\n", j);
            // Ensure that NTAB is a power of two.
            // Clamp j.
            j &= NTAB - 1;
        }

        m_iy = m_iv[j];
        m_iv[j] = m_idum;

        return m_iy;
    }

    public float RandomFloat(float flLow, float flHigh)
    {
        // float in [0,1)
        float fl = AM * GenerateRandomNumber();
        if (fl > RNMX)
        {
            fl = RNMX;
        }
        return (fl * (flHigh - flLow)) + flLow; // float in [low,high)
    }

    public float RandomFloatExp(float flMinVal, float flMaxVal, float flExponent)
    {
        // float in [0,1)
        float fl = AM * GenerateRandomNumber();
        if (fl > RNMX)
        {
            fl = RNMX;
        }
        if (flExponent != 1.0f)
        {
            fl = Mathf.Pow(fl, flExponent);
        }
        return (fl * (flMaxVal - flMinVal)) + flMinVal; // float in [low,high)
    }

    public int RandomInt(int iLow, int iHigh)
    {
        //ASSERT(lLow <= lHigh);
        uint maxAcceptable;
        uint x = (uint)(iHigh - iLow + 1);
        uint n;
        if (x <= 1 || MAX_RANDOM_RANGE < x - 1)
        {
            return iLow;
        }

        // The following maps a uniform distribution on the interval [0,MAX_RANDOM_RANGE]
        // to a smaller, client-specified range of [0,x-1] in a way that doesn't bias
        // the uniform distribution unfavorably. Even for a worst case x, the loop is
        // guaranteed to be taken no more than half the time, so for that worst case x,
        // the average number of times through the loop is 2. For cases where x is
        // much smaller than MAX_RANDOM_RANGE, the average number of times through the
        // loop is very close to 1.
        //
        maxAcceptable = (uint)(MAX_RANDOM_RANGE - ((MAX_RANDOM_RANGE + 1) % x));
        do
        {
            n = (uint)GenerateRandomNumber();
        } while (n > maxAcceptable);

        return iLow + (int)(n % x);
    }
}
