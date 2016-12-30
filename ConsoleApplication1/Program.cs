using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monsters
{
    public class Dice {
        private Random random;

        public Dice() {
            random = new Random();
        }
        public int tirageD6() {
            return random.Next(1, 7);            
        }
        public int tirageD100() {
            return random.Next(1, 101);
        }
    }

    public class Player {
        public String name { get; private set; }
        public int pv { get; set; }
        public int score { get; set; }

        public bool dead { get { return pv <= 0;}
            private set {} } 

        public Player(String name) {
            this.name = name;
            pv = 150;
            score = 0;
            dead = false;
        }

        public int damage(int damages) {
            return pv -= damages;
        }
        
    }

    public abstract class Monster {
        public bool dead { get; set; }
        public Monster() {
            dead = false;
        }

        public abstract int getGain();

        public abstract int getSpellRatio();
                       
    }

    public class MonsterHard : Monster {
        public override int getGain() {
            return 2;
        }

        public override int getSpellRatio() {
            return 5;
        }
    }

    public class MonsterEasy : Monster {

        public override int getGain() {
            return 1;
        }

        public override int getSpellRatio() {
            return 0;
        }
    }

    public class Game {
        private Dice dice;
        private const int ratioSpell = 5;
        private const int damages = 10;

        public Game() {
            dice = new Dice();
        }

        private Monster getMonster() {
            if (dice.tirageD100() % 3 == 0)
                return new MonsterHard();
            else
                return new MonsterEasy();

        }

        public void run() {
            Console.WriteLine("Bonjour. Veuillez entrer votre nom :");            
            Player p = new Player(Console.ReadLine());
            Monster m = null;
            int nbMonsterEasy = 0;
            int nbMonsterHard = 0;
            while (!p.dead) {
                if (m == null || m.dead)
                    m = getMonster();
                fight(p, m);
                if (m is MonsterEasy)
                    nbMonsterEasy++;
                else
                    nbMonsterHard++;
            }
            Console.WriteLine(p.name + " n'est plus. Son score est de " + p.score + " points.");
            Console.WriteLine(p.name + " a glorieusement combattu. Il a vaincu " + (nbMonsterHard + nbMonsterEasy) + " vilaines bêtes");
            Console.WriteLine(nbMonsterEasy + " vilaines bêtes");
            Console.WriteLine(nbMonsterHard + " très vilaines bêtes");
        }

        private void fight(Player p, Monster m) {
            while (!m.dead && !p.dead) {
                //Attaque player
                int pDice = dice.tirageD6();
                int mDice = dice.tirageD6();
                m.dead = pDice >= mDice;

                if (!m.dead) {//Attaque monster 
                    pDice = dice.tirageD6();
                    mDice = dice.tirageD6();
                    int sumDamages = 0;
                    if (mDice > pDice) {
                        pDice = dice.tirageD6();//jet de bouclier
                        if (pDice > 2) {
                            p.damage(damages);//Dégats
                            sumDamages += damages;
                        }
                    }
                    //chain spell
                    int spellRatio = m.getSpellRatio();
                    if (spellRatio > 0) {
                        mDice = dice.tirageD6();
                        if (mDice == 6) {//Succès
                            p.damage(mDice * ratioSpell);//Dégats
                            sumDamages += mDice * ratioSpell;
                        }
                    }
                    if (sumDamages > 0)
                        Console.WriteLine("Echec du combat " + sumDamages + " PV perdus");
                }
                else { //Gains
                    Console.WriteLine("Succès du combat " + m.getGain() + " points gagnés");
                    p.score += m.getGain();
                }
            }

        }
    }

    class Program {        
        static void Main(string[] args) {
                Game g = new Game();
                g.run();
        }
    }
}
