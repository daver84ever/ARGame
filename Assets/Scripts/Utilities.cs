using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

public static class Utilities {

	public static string ColorToHex(this Color32 color){
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}
	 
	public static Color HexToColor(this string hex){
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}

	public static float Avg(params float[] nums){
		float ans = 0;
		for(int i = 0; i < nums.Length; i++){
			ans += nums[i];
		}
		return ans / nums.Length;
	}

	public static void SetParentZeroed(this Transform transform, Transform parent){
		transform.parent = parent;
		transform.localScale = Vector3.one;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	public static Vector3 RandomVector3{
		get { return new Vector3(Random.value, Random.value, Random.value); }
	}
	public static Vector2 RandomVector2{
		get { return new Vector2(Random.value, Random.value); }
	}

	public static void Raise<T>(this System.EventHandler<T> handler, object sender, T args) where T : System.EventArgs {
    	if (handler != null) handler(sender, args);
 	 }
	public static void Raise(this System.EventHandler handler, object sender, System.EventArgs args) {
    	if (handler != null) handler(sender, args);
 	 }
	public static void Raise(this System.EventHandler handler, object sender){
    	if (handler != null) handler(sender, System.EventArgs.Empty);
	}

	public static Vector3 RandomVector3Range(float min, float max){
		return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
	}

	public static Color ColorSmoothStep(Color from, Color to, float t){
		//float r = from.r; float g = from.g; float b = from.b; float a = from.a;
		float r = Mathf.SmoothStep(from.r, to.r, t);
		float g = Mathf.SmoothStep(from.g, to.g, t);
		float b = Mathf.SmoothStep(from.b, to.b, t);
		float a = Mathf.SmoothStep(from.a, to.a, t);
		return new Color(r, g, b, a);
	}

	public static Vector4 Vector4SmoothDampUnScaled(Vector4 current, Vector4 target, ref Vector4 velocity, float smoothTime){
		Vector4 ans = Vector4.zero;
		ans.x = Mathf.SmoothDamp(current.x, target.x, ref velocity.x, smoothTime, Mathf.Infinity, Utilities.unscaledDeltaTime);
		ans.y = Mathf.SmoothDamp(current.y, target.y, ref velocity.y, smoothTime, Mathf.Infinity, Utilities.unscaledDeltaTime);
		ans.z = Mathf.SmoothDamp(current.z, target.z, ref velocity.z, smoothTime, Mathf.Infinity, Utilities.unscaledDeltaTime);
		ans.w = Mathf.SmoothDamp(current.w, target.w, ref velocity.w, smoothTime, Mathf.Infinity, Utilities.unscaledDeltaTime);
		return ans;
	}

	public static Vector4 Vector4SmoothDamp(Vector4 current, Vector4 target, ref Vector4 velocity, float smoothTime){
		Vector4 ans = Vector4.zero;
		ans.x = Mathf.SmoothDamp(current.x, target.x, ref velocity.x, smoothTime);
		ans.y = Mathf.SmoothDamp(current.y, target.y, ref velocity.y, smoothTime);
		ans.z = Mathf.SmoothDamp(current.z, target.z, ref velocity.z, smoothTime);
		ans.w = Mathf.SmoothDamp(current.w, target.w, ref velocity.w, smoothTime);
		return ans;
	}


	public static Vector4 Vector4SmoothDamp(Vector4 current, Vector4 target, ref float velocity, float smoothTime){
		bool magGreater = (target.magnitude > current.magnitude);
		float acceleration = Mathf.Abs((target-current).magnitude) / (smoothTime*smoothTime / 4);
		velocity += acceleration;
		float maxvelocity = Mathf.Abs((target-current).magnitude / smoothTime);
		if (velocity > maxvelocity) velocity = maxvelocity;
		Vector4 endPos = Vector4.MoveTowards(current, target, velocity * Time.deltaTime);
		if (magGreater){
			if (endPos.magnitude > target.magnitude){
				return target;
			}
		}
		else if (endPos.magnitude < target.magnitude){
			return target;
		}
		return endPos;
	}

	public static Vector2 Vector2SmoothStep(Vector2 from, Vector2 to, float t){
		float x = Mathf.SmoothStep(from.x, to.x, t);
		float y = Mathf.SmoothStep(from.y, to.y, t);
		return new Vector2(x, y);
	}

	public static Vector3 ConvertV2xy(Vector2 point){
		return new Vector3(point.x, point.y, .25f);
	}

	public static Vector3 ConvertV2xz(Vector2 point){
		return new Vector3(point.x, .25f, point.y);
	}
	public static Vector2 ConvertV3xz(Vector3 point){
		return new Vector2(point.x, point.z);
	}

	public static float Vector2Cross(Vector2 p1, Vector2 p2) {
		return p1.x * p2.y - p1.y * p2.x;
	}

	public static Rect CenterRect(Vector2 center, float width, float height){
		return new Rect(
			center.x-width/2,
			center.y-height/2,
			width, height);
	}

	public static Vector3 SwapYZ(Vector3 vector){
		return new Vector3(vector.x, vector.z, vector.y);
	}

	public static void Add<T>(this IList<T> list, params T[] objects){
		for(int i = 0; i < objects.Length; i++){
			list.Add(objects[i]);
		}
	}

	public static T[] Concat<T>(this T[] array, T[] toAppend){
		int oldlen = array.Length;
		System.Array.Resize(ref array, array.Length + toAppend.Length);
		for(int i = oldlen; i < array.Length; i++){
			array[i] = toAppend[i - oldlen];
		}
		return array;
	}

	public static void Shuffle<T>(this IList<T> list)  
	{  
	    System.Random rng = new System.Random();  
	    int n = list.Count;  
	    while (n > 1) {  
	        n--;  
	        int k = rng.Next(n + 1);  
	        T value = list[k];  
	        list[k] = list[n];  
	        list[n] = value;  
	    }  
	}


	public static void SetActiveRecursively(GameObject rootObject, bool active)
        {
            rootObject.SetActive(active);
            foreach (Transform childTransform in rootObject.transform)
            {
                SetActiveRecursively(childTransform.gameObject, active);
            }
        }

	public static string SplitCamelCase(this string str)
{
    return Regex.Replace( 
        Regex.Replace( 
            str, 
            @"(\P{Ll})(\P{Ll}\p{Ll})", 
            "$1 $2" 
        ), 
        @"(\p{Ll})(\P{Ll})", 
        "$1 $2" 
    );
}

	public static Color SaturateColor(Color color, float change) {

		float R = color.r;
		float G = color.g;
		float B = color.b;
		float A = color.a;
		
		float P=Mathf.Sqrt(
			(R)*(R)*.299f+
			(G)*(G)*.587f+
			(B)*(B)*.114f ) ;
		
		R=P+((R)-P)*change;
		G=P+((G)-P)*change;
		B=P+((B)-P)*change; 
		return new Color(R, G, B, A);
	}


	public static Color AverageColors(params Color[] colors){
		if (colors.Length < 1) return Color.black;
		if (colors.Length == 1) return colors[0];
		float r, g, b, a;
		r = g = b = a = 0;
		for(int i = 0; i < colors.Length; i++){
			r += colors[i].r;
			g += colors[i].g;
			b += colors[i].b;
			a += colors[i].a;
		}
		int count = colors.Length;
		return new Color(r/count, g/count, b/count, a/count);
	}

	public static Color AverageColors(List<Color> colors){
		if (colors.Count < 1) return Color.black;
		if (colors.Count == 1) return colors[0];
		float r, g, b, a;
		r = g = b = a = 0;
		for(int i = 0; i < colors.Count; i++){
			r += colors[i].r;
			g += colors[i].g;
			b += colors[i].b;
			a += colors[i].a;
		}
		int count = colors.Count;
		return new Color(r/count, g/count, b/count, a/count);
	}

	public static void SetMeshColors(GameObject gameObject, Color c){
		Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
		SetMeshColors(ref mesh, c);
	}

	public static void SetMeshColors(ref Mesh mesh, Color c){
		Color32[] colors = new Color32[mesh.vertexCount];
		for(int i = 0; i < colors.Length; i++){
			colors[i] = c;
//			colors[i] = new Color(Random.value, Random.value, Random.value, 1f);
		}
		mesh.colors32 = colors;
	}



	public static bool IsWhiteSpace(char c){
		return (c == ' ');
	}


	public static string WordWrap( string the_string, int width ) {
		string _newline = "\r\n";
		int pos, next;
		StringBuilder sb = new StringBuilder();
		
		// Lucidity check
		if ( width < 1 )
			return the_string;
		
		// Parse each line of text
		for ( pos = 0; pos < the_string.Length; pos = next ) {
			// Find end of line
			int eol = the_string.IndexOf( _newline, pos );
			
			if ( eol == -1 )
				next = eol = the_string.Length;
			else
				next = eol + _newline.Length;
			
			// Copy this line of text, breaking into smaller lines as needed
			if ( eol > pos ) {
				do {
					int len = eol - pos;
					
					if ( len > width )
						len = BreakLine( the_string, pos, width );
					
					sb.Append( the_string, pos, len );
					sb.Append( _newline );
					
					// Trim whitespace following break
					pos += len;
					
					while ( pos < eol && IsWhiteSpace( the_string[pos] ) )
						pos++;
					
				} while ( eol > pos );
			} else sb.Append( _newline ); // Empty line
		}
		
		return sb.ToString();
	}
	
	/// <summary>
	/// Locates position to break the given line so as to avoid
	/// breaking words.
	/// </summary>
	/// <param name="text">String that contains line of text</param>
	/// <param name="pos">Index where line of text starts</param>
	/// <param name="max">Maximum line length</param>
	/// <returns>The modified line length</returns>
	public static int BreakLine(string text, int pos, int max)
	{
		// Find last whitespace in line
		int i = max - 1;
		while (i >= 0 && !IsWhiteSpace(text[pos + i]))
			i--;
		if (i < 0)
			return max; // No whitespace found; break at maximum length
		// Find start of whitespace
		while (i >= 0 && IsWhiteSpace(text[pos + i]))
			i--;
		// Return length of text before whitespace
		return i + 1;
	}

    public static float unscaledDeltaTime
    {
//        get { return Time.timeScale == 0 ? Time.deltaTime : Time.deltaTime / Time.timeScale; }
		get{return Time.unscaledDeltaTime;}
    }

	public static Color ToGammaSpace(this Color c){
		return new Color(Mathf.LinearToGammaSpace(c.r),
		                 Mathf.LinearToGammaSpace(c.g),
		                 Mathf.LinearToGammaSpace(c.b),
		                 Mathf.LinearToGammaSpace(c.a));
	}
	public static Color ToLinearSpace(this Color c){
		return new Color(Mathf.GammaToLinearSpace(c.r),
		                 Mathf.GammaToLinearSpace(c.g),
		                 Mathf.GammaToLinearSpace(c.b),
		                 Mathf.GammaToLinearSpace(c.a));
	}

	public static Vector3 GetIntersection2D(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4){
		float a = Det(p1.x, p1.z, p2.x, p2.z);
		float c = Det(p3.x, p3.z, p4.x, p4.z);
		float x1x2 = p1.x - p2.x;
		float x3x4 = p3.x - p4.x;
		float y1y2 = p1.z - p2.z;
		float y3y4 = p3.z - p4.z;
		float denom = Det (x1x2, y1y2, x3x4, y3y4);
		return new Vector3(Det(a, x1x2, c, x3x4)/denom, p1.y, Det(a, y1y2, c, y3y4)/denom);
	}

	public static float Det(float a, float b, float c, float d){
		return a*d - b*c;
	}

	public static float RelativeAngle(Vector3 forward, Vector3 direction){
		Vector3 referenceRight= Vector3.Cross(Vector3.up, forward);
		return Mathf.Sign(Vector3.Dot(direction, referenceRight)) * Vector3.Angle(direction, forward);
	}


	public static float CalcDampedSimpleHarmonicMotion
		(
			ref float pPos,            // position value to update
			ref float pVel,            // velocity value to update
			float equilibriumPos,   // position to approach
			float deltaTime,        // time to update over
			float angularFrequency, // angular frequency of motion
			float dampingRatio      // damping ratio of motion
			)
	{
		const float epsilon = 0.0001f;
		
		// if there is no angular frequency, the spring will not move
		if (angularFrequency < epsilon)
			return pPos;
		
		// clamp the damping ratio in legal range
		if (dampingRatio < 0.0f)
			dampingRatio = 0.0f;
		
		// calculate initial state in equilibrium relative space
		float initialPos = pPos - equilibriumPos;
		float initialVel = pVel;
		
		// if over-damped
		if (dampingRatio > 1.0f + epsilon)
		{
			// calculate constants based on motion parameters
			// Note: These could be cached off between multiple calls using the same
			//       parameters for deltaTime, angularFrequency and dampingRatio.
			float za = -angularFrequency * dampingRatio;
			float zb = angularFrequency * Mathf.Sqrt(dampingRatio*dampingRatio - 1.0f);
			float z1 = za - zb;
			float z2 = za + zb;
			float expTerm1 = Mathf.Exp( z1 * deltaTime );
			float expTerm2 = Mathf.Exp( z2 * deltaTime );
			
			// update motion
			float c1 = (initialVel - initialPos*z2) / (-2.0f*zb); // z1 - z2 = -2*zb
			float c2 = initialPos - c1;
			pPos = equilibriumPos + c1*expTerm1 + c2*expTerm2;
			pVel = c1*z1*expTerm1 + c2*z2*expTerm2;
		}
		// else if critically damped
		else if (dampingRatio > 1.0f - epsilon)
		{
			// calculate constants based on motion parameters
			// Note: These could be cached off between multiple calls using the same
			//       parameters for deltaTime, angularFrequency and dampingRatio.
			float expTerm = Mathf.Exp( -angularFrequency * deltaTime );
			
			// update motion
			float c1 = initialVel + angularFrequency * initialPos;
			float c2 = initialPos;
			float c3 = (c1*deltaTime + c2) * expTerm;
			pPos = equilibriumPos + c3;
			pVel = (c1*expTerm) - (c3*angularFrequency);
		}
		// else under-damped
		else
		{
			// calculate constants based on motion parameters
			// Note: These could be cached off between multiple calls using the same
			//       parameters for deltaTime, angularFrequency and dampingRatio.
			float omegaZeta = angularFrequency * dampingRatio;
			float alpha = angularFrequency * Mathf.Sqrt(1.0f - dampingRatio*dampingRatio);
			float expTerm = Mathf.Exp( -omegaZeta * deltaTime );
			float cosTerm = Mathf.Cos( alpha * deltaTime );
			float sinTerm = Mathf.Sin( alpha * deltaTime );
			
			// update motion
			float c1 = initialPos;
			float c2 = (initialVel + omegaZeta*initialPos) / alpha;
			pPos = equilibriumPos + expTerm*(c1*cosTerm + c2*sinTerm);
			pVel = -expTerm*( (c1*omegaZeta - c2*alpha)*cosTerm +
			                  (c1*alpha + c2*omegaZeta)*sinTerm);
		}
		return pPos;
	}

	public static float CalcDampedSimpleHarmonicMotion
		(
			float pPos,            // position value to update
			ref float pVel,            // velocity value to update
			float equilibriumPos,   // position to approach
			float deltaTime,        // time to update over
			float angularFrequency, // angular frequency of motion
			float dampingRatio      // damping ratio of motion
			)
	{
		const float epsilon = 0.0001f;
		
		// if there is no angular frequency, the spring will not move
		if (angularFrequency < epsilon)
			return pPos;
		
		// clamp the damping ratio in legal range
		if (dampingRatio < 0.0f)
			dampingRatio = 0.0f;
		
		// calculate initial state in equilibrium relative space
		float initialPos = pPos - equilibriumPos;
		float initialVel = pVel;
		
		// if over-damped
		if (dampingRatio > 1.0f + epsilon)
		{
			// calculate constants based on motion parameters
			// Note: These could be cached off between multiple calls using the same
			//       parameters for deltaTime, angularFrequency and dampingRatio.
			float za = -angularFrequency * dampingRatio;
			float zb = angularFrequency * Mathf.Sqrt(dampingRatio*dampingRatio - 1.0f);
			float z1 = za - zb;
			float z2 = za + zb;
			float expTerm1 = Mathf.Exp( z1 * deltaTime );
			float expTerm2 = Mathf.Exp( z2 * deltaTime );
			
			// update motion
			float c1 = (initialVel - initialPos*z2) / (-2.0f*zb); // z1 - z2 = -2*zb
			float c2 = initialPos - c1;
			pPos = equilibriumPos + c1*expTerm1 + c2*expTerm2;
			pVel = c1*z1*expTerm1 + c2*z2*expTerm2;
		}
		// else if critically damped
		else if (dampingRatio > 1.0f - epsilon)
		{
			// calculate constants based on motion parameters
			// Note: These could be cached off between multiple calls using the same
			//       parameters for deltaTime, angularFrequency and dampingRatio.
			float expTerm = Mathf.Exp( -angularFrequency * deltaTime );
			
			// update motion
			float c1 = initialVel + angularFrequency * initialPos;
			float c2 = initialPos;
			float c3 = (c1*deltaTime + c2) * expTerm;
			pPos = equilibriumPos + c3;
			pVel = (c1*expTerm) - (c3*angularFrequency);
		}
		// else under-damped
		else
		{
			// calculate constants based on motion parameters
			// Note: These could be cached off between multiple calls using the same
			//       parameters for deltaTime, angularFrequency and dampingRatio.
			float omegaZeta = angularFrequency * dampingRatio;
			float alpha = angularFrequency * Mathf.Sqrt(1.0f - dampingRatio*dampingRatio);
			float expTerm = Mathf.Exp( -omegaZeta * deltaTime );
			float cosTerm = Mathf.Cos( alpha * deltaTime );
			float sinTerm = Mathf.Sin( alpha * deltaTime );
			
			// update motion
			float c1 = initialPos;
			float c2 = (initialVel + omegaZeta*initialPos) / alpha;
			pPos = equilibriumPos + expTerm*(c1*cosTerm + c2*sinTerm);
			pVel = -expTerm*( (c1*omegaZeta - c2*alpha)*cosTerm +
			                  (c1*alpha + c2*omegaZeta)*sinTerm);
		}
		return pPos;
	}

	public static Quaternion SimpleHarmonicMotion(Quaternion current, Quaternion target, ref Vector4 velocity, float angularFrequency, float dampingRatio){
		Quaternion ans = new Quaternion(
			CalcDampedSimpleHarmonicMotion(ref current.x, ref velocity.x, target.x, Time.deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.y, ref velocity.y, target.y, Time.deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.z, ref velocity.z, target.z, Time.deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.w, ref velocity.w, target.w, Time.deltaTime, angularFrequency, dampingRatio)
		);
		return ans;

	}
	public static Color SimpleHarmonicMotion(Color current, Color target, ref Color velocity, float deltaTime, float angularFrequency, float dampingRatio){
		Color ans = new Color(
			CalcDampedSimpleHarmonicMotion(ref current.r, ref velocity.r, target.r, deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.g, ref velocity.g, target.g, deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.b, ref velocity.b, target.b, deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.a, ref velocity.a, target.a, deltaTime, angularFrequency, dampingRatio)
		);
		return ans;
	}

	public static Vector4 SimpleHarmonicMotion(Vector4 current, Vector4 target, ref Vector4 velocity, float angularFrequency, float dampingRatio){
		Vector4 ans = new Vector4(
			CalcDampedSimpleHarmonicMotion(ref current.x, ref velocity.x, target.x, Time.deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.y, ref velocity.y, target.y, Time.deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.z, ref velocity.z, target.z, Time.deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.w, ref velocity.w, target.w, Time.deltaTime, angularFrequency, dampingRatio)
		);
		return ans;
	}
	public static Vector3 SimpleHarmonicMotion(Vector3 current, Vector3 target, ref Vector3 velocity, float angularFrequency, float dampingRatio){
		Vector3 ans = new Vector3(
			CalcDampedSimpleHarmonicMotion(ref current.x, ref velocity.x, target.x, Time.deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.y, ref velocity.y, target.y, Time.deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.z, ref velocity.z, target.z, Time.deltaTime, angularFrequency, dampingRatio)
		);
		return ans;
	}
	public static Vector3 SimpleHarmonicMotion(Vector3 current, Vector3 target, ref Vector3 velocity, float angularFrequency, float dampingRatio, float deltaTime){
		Vector3 ans = new Vector3(
			CalcDampedSimpleHarmonicMotion(ref current.x, ref velocity.x, target.x, deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.y, ref velocity.y, target.y, deltaTime, angularFrequency, dampingRatio),
			CalcDampedSimpleHarmonicMotion(ref current.z, ref velocity.z, target.z, deltaTime, angularFrequency, dampingRatio)
		);
		return ans;
	}

}

