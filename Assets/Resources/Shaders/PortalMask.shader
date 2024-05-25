Shader"Custom/PortalMask"
{
    Subshader
    {
        ZWrite off
        ColorMask 0

        Stencil
        {
            Ref 1
            Comp Always
            Pass replace
        }

        Pass
        {

        }
    }
}
