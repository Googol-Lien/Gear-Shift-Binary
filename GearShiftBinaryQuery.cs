//==================================================================
// Modified from: https://gist.github.com/tzengyuxio/5478a1f29cb2a8532238#file-binarystatetoindex-cpp
// and: https://gist.github.com/tzengyuxio/554c26d9300e455438be
//範例程式碼， Unity3D, C#:

const int NUM_DIGITS = 7;

long C( int n, int r )
{
	if ( n < r ) return 0;
	if ( n <= 0 ) return 0;
	if ( r == 0 || n == r ) return 1;

	int n2 = n - r;

	if ( n2 < r ) { r = n2; }

	long ans = 1;

	//n!/(n-r)! = n * (n-1) * (n-2)....*(n-(r-1)) [(n -r)! 以下被除掉了]
	for ( int i = 0; i < r; ++i )
	{
		ans *= ( n - i );
	}

	//除以 r!
	for ( int i = 0; i < r; ++i )
	{
		ans /= ( i + 1 );
	}

	return ans;
}


long ConvertBinaryStateToIndex( int bs, int numDigits )
{
	int oneCount = TPMath.BTCOUNT( ( uint ) bs );
	long _base = 0;
	long inGroup = 0;

	for ( int i = 1; i < oneCount; ++i )
	{
		_base += C( numDigits, i );
	}

	int oneRemain = oneCount;

	for ( int i = numDigits - 1; i >= 0; --i )
	{
		if ( ( bs & ( 1<< i ) ) != 0 )
		{
			if ( i + 1 == oneRemain )
			{
				break;
			}

			inGroup += C( i, oneRemain );
			oneRemain--;
		}
	}

	if ( oneCount > 0 ) inGroup++;

	return _base + inGroup;
}


long ConvertIndexToBinaryState( int idx, int numDigits )
{
	int oneCount = 0;
	long inGroup = idx;

	//afther this loop, we can get the values of oneCount and inGroup
	for ( int i = 0; i <= numDigits; i++ )
	{
		if ( inGroup >= C( numDigits, i ) )
		{
			oneCount++;
			inGroup -= C( numDigits, i );
		}
		else
		{
			break;
		}
	}

	//generate binary
	long s = 0;

	for ( int i = numDigits - 1; i >= 0; i-- )
	{
		s <<= 1;
		if ( oneCount != 0 && inGroup >= C( i, oneCount ) )
		{
		    s |= 1;
		    inGroup -= C( i, oneCount );
		    oneCount--;
		}
	}

	return s;
}

void Start()
{
	int max = (1 << NUM_DIGITS);

	for ( int i = 0; i < max; ++i )
	{
	    Debug.Log( i + " = " +  Convert.ToString( i, 2 ).PadLeft( 7, '0' ) + " => " + ConvertBinaryStateToIndex( i, NUM_DIGITS ) );
	}

	long dig;

	for ( int i = 0; i < max; ++i )
	{
		dig = ConvertIndexToBinaryState( i, NUM_DIGITS );

		Debug.Log( i + " => " + dig + " = " + Convert.ToString( dig, 2 ).PadLeft( 7, '0' ) );
	}
}

//==================================================================
