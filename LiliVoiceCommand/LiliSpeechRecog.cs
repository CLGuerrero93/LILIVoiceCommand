//Microsoft.Speech must be added to References before compilation
using System;
using System.Collections;
using Microsoft.Speech.Recognition;

namespace SpeechRecognitionApp
{
    public class Program
    {
        public Queue qCommands = new Queue();  //Holds the recognized phrase
        public string[] phrases = new string[] {"Lily", "Move right", "Move left", "Follow me", "Stop", "Goodbye"}; //array of possible phrases
        public bool Listening = false; //if Speech Recognition is actively listening
        
        // returns the SpeechRecognitionEngine object
        public SpeechRecognitionEngine buildRecognizer()
        {
            // Create a SpeechRecognitionEngine object for the default recognizer in the en-US locale.
            SpeechRecognitionEngine recognizer =
              new SpeechRecognitionEngine(
                new System.Globalization.CultureInfo("en-US"));
            {

                // Create a grammar for different commands.
                Choices gestures = new Choices(new string[] { "Lily", "Move right", "Move left", "Follow me", "Stop", "Goodbye" });

                GrammarBuilder giveCommands = new GrammarBuilder(gestures);

                // Create a Grammar object from the GrammarBuilder and load it to the recognizer.
                Grammar commandsGrammar = new Grammar(giveCommands);
                recognizer.LoadGrammarAsync(commandsGrammar);

                // Add a handler for the speech recognized event.
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(recognizedSpeech);

                // Configure the input to the speech recognizer.
                recognizer.SetInputToDefaultAudioDevice();

                return recognizer;
            }
        }

        // Start Speech Recognition.
        public void runRecognizer(SpeechRecognitionEngine recognizer)
        {
            Listening = true;
            // Start asynchronous, continuous speech recognition.
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        // Handle the SpeechRecognized event.
        public void recognizedSpeech(object sender, SpeechRecognizedEventArgs e)
        {
            // Find the recognized phrase in the array phrases
            for (int index = 0; index < 6; index++)
            {
                if (0==e.Result.Text.CompareTo(phrases[index]))
                {
                    // Place the index of the recognized phrase at the end of the Queue
                    qCommands.Enqueue(index);
                    break;
                }
            }
        }

        // Send from the Queue
        public int grabCommand()
        {
            // If there are values in the queue:
            int emp = qCommands.Count;
            if (emp != 0)
            {
                // Send and remove the value at the beginning of the queue
                int command = (int)qCommands.Dequeue();
                return command;
            }

            // If the queue is empty:
            return -1;
        }

        // Stop Speech Recognition
        public void stopListening(SpeechRecognitionEngine recognizer)
        {
            recognizer.RecognizeAsyncStop();
            Listening = false;
        }
    }
}