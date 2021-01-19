using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNeuralNetwork
{
    enum categories
    {
        zero,
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine

    }
    class Category
    {

        
        List<LabeledImage> training = new List<LabeledImage>();
        List<LabeledImage> testing = new List<LabeledImage>();
        //categories label;

        public Category(List<byte> data, categories label, int offset)
        {
            SplitData(offset, data, label);

        }

        public List<LabeledImage> GetTraining()
        {
            return this.training;
        }

        public void SetTraining(List<LabeledImage> training)
        {
            this.training = training;
        }

        public List<LabeledImage> GetTesting()
        {
            return this.testing;
        }

        public void SetTesting(List<LabeledImage> testing)
        {
            this.testing = testing;
        }

        private void SplitData(int offset, List<byte> data, categories label)
        {
            int total = data.Count / offset;
            Console.WriteLine("Total images: " + total);

            for (int i = 0; i < total; i++)             //Iteracja obrazow
            {
                if (i < Math.Floor(0.7 * total))
                {
                    training.Add(new LabeledImage());
                }
                else
                {
                    testing.Add(new LabeledImage());
                }

                for (int j = 0; j < offset; j++)        //Iteracja pixeli w obrazie
                {
                    if(i < Math.Floor(0.7 * total))
                    {
                        training.Last().pixels.Add(data[j + (offset * i)]);
                        training.Last().category = label;
                        
                    } else
                    {
                        testing.Last().pixels.Add(data[j + (offset * i)]);
                        testing.Last().category = label;
                    }
                    
                }
            }
        }
    }
}
