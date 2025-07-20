using System.Collections.Generic;

namespace DirekcijaTrke.PomocneFunkcije
{
    public class NajbrziKrug
    {

        public double ProveraNajbrzegKruga(List<double> vremenaPoKrugu)
        {

            double najboljeVreme = vremenaPoKrugu[0];

            for (int i = 1; i < vremenaPoKrugu.Count; i++)
            {
                if (najboljeVreme > vremenaPoKrugu[i])
                {
                    najboljeVreme = vremenaPoKrugu[i];
                }
            }

            return najboljeVreme;

        }

    }
}
