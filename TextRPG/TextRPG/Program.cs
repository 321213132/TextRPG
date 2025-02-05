using Microsoft.VisualBasic;
using System.Numerics;
using System.Runtime.Serialization.Json;

namespace TextRPG
{
    public class Player
    {
        public string PName;//이름
        public string PClass;//직업
        public string PLv;//캐릭터 정보에서 레벨 자릿수 보정

        public int PLevel = 1;//레벨
        public int PAtk = 10;//공격력
        public int PDef = 5;//방어력
        public int PHealth = 100;//체력
        public int PGold = 1500;//골드

        public int PInput;
    }
    public class Item
    {
        public string Name { get; }//이름
        public int Type { get; }//갑옷 - 0, 무기 - 1
        public string Data { get; }//설명
        public int ATK { get; }//공격력
        public int DEF { get; }//방어력
        public int Gold { get; }//가격
        public bool IsEquipped { get; set; }//장착 유무
        public bool IsPurchased { get; set; }//구매 유무

        public static int ItemCount = 0;//AddItem을 사용하기위한 변수
        public int equIpcount {  get; set; }

        public Item(string name, int type, string data, int atk, int def, int gold, bool isEquipped = false, int equipcount = 0)
        {
            Name = name; //이름
            Type = type; //갑옷, 무기
            Data = data; //설명
            ATK = atk;   //공격력
            DEF = def;   //방어력
            Gold = gold; //가격
            IsEquipped = isEquipped; //장착유무
            IsPurchased = false;     //구매유무
            equIpcount = equipcount;
        }
        //인벤토리,상점에 출력
        public void ItemDescription(int index, bool withNum, bool showPrice)//번호 유무, 가격 공개 유무
        {
            Console.Write("- ");
            //번호
            if(withNum)
            {
                Console.Write("{0}. ", index);
            }
            //장착유무
            if (IsEquipped)
            {
                Console.Write("[E] ");
            }
            //이름
            Console.Write($"{Name}\t| ");
            //공격력,방어력 방어력 - 0 공격력 - 1
            if (Type == 0) { Console.Write("방어력 +{0}\t| ", DEF); }
            if (Type == 1) { Console.Write("공격력 +{0}\t| ", ATK); }
            //설명
            Console.Write($"{Data}\t| ");
            //가격, 구매완료
            if(showPrice)//가격보여주기
            {
                if(IsPurchased)//구매유무 확인
                {
                    Console.Write("구매 완료");
                }
                else
                {
                    Console.Write($"{Gold} G");
                }
            }
            Console.WriteLine(equIpcount);

            Console.WriteLine();

        }
    }

    class Program
    {
        static Item[] items;
        static Player player = new Player();

        static int minNumber, maxNumber;//각 화면마다 작동하는 최소,최대번호를 확인해 
                                        //문자나 이외의 숫자를 기입했을 때 재입력하도록 만들기위함
        static int baseAtk = player.PAtk, baseDef = player.PDef;//기본 스텟 저장
        static void Main(string[] args)
        {
            DataSetting();

            PlayerSetting();

        }
        //이름과 직업을 설정
        private static void PlayerSetting()
        {
            minNumber = 1; maxNumber = 2;
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다!");
            Console.WriteLine("원하는 이름을 설정해주세요.\n");

            for (int i = 0; i == 0;)
            {
                player.PName = Console.ReadLine();

                Console.WriteLine($"\n입력하신 이름은 {player.PName} 입니다\n");
                Console.WriteLine("1. 저장\n2. 취소\n");
                Console.WriteLine("원하시는 행동을 입력하세요\n");
                InputPlayer(minNumber, maxNumber);
                switch (player.PInput)
                {
                    case 1:
                        i++;
                        Console.Clear();
                        break;
                    case 2:
                        Console.WriteLine("이름을 다시 입력해주세요\n");
                        break;
                }
            }

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다!");
            Console.WriteLine("원하는 직업을 선택해주세요.\n");
            Console.WriteLine("1. 전사\n2. 도적\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            InputPlayer(minNumber, maxNumber);
            switch (player.PInput)
            {
                case 1:
                    player.PClass = "전사";
                    break;
                case 2:
                    player.PClass = "도적";
                    break;
            }
            Console.Clear();
            //게임 시작화면 함수
            StartMenu();

        }
        //게임 시작 화면
        private static void StartMenu()
        {
            minNumber = 1;  maxNumber = 3;
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다!");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1. 상태창\n2. 인벤토리\n3. 상점\n");

            Console.WriteLine("원하시는 행동을 입력해주세요.");
            InputPlayer(minNumber, maxNumber);
            Console.Clear();
            switch (player.PInput)
            {
                case 1:
                    Status();
                    break;
                case 2:
                    Inventory();
                    break;
                case 3:
                    Shop();
                    break;
            }

        }

        //상태창
        private static void Status()
        {
            minNumber = 0; maxNumber = 0;
            //캐릭터의 정보 표시
            Console.WriteLine("상태창\n캐릭터의 정보가 표시됩니다.\n");
            //레벨/이름(직업)/공격력/방어력/체력/Gold-기본값은 동일하게
            player.PLv = player.PLevel.ToString("D2");//2자릿수로 맞추기
            Console.WriteLine($"Lv. {player.PLv}");                 //레벨
            Console.WriteLine($"{player.PName} ({player.PClass})"); //이름 직업
            Console.Write($"공격력 : {player.PAtk}");           //공격력
            if(player.PAtk != baseAtk)
            {
                Console.Write(" (+{0})",player.PAtk - baseAtk);
            }

            Console.Write($"\n방어력 : {player.PDef}");           //방어력
            if (player.PDef != baseDef)
            {
                Console.Write(" (+{0})", player.PDef - baseDef);
            }
            Console.WriteLine($"\n체 력 : {player.PHealth}");         //체력
            Console.WriteLine($"Gold : {player.PGold} G");          //Gold

            Console.WriteLine("\n0. 나가기\n");

            Console.WriteLine("원하시는 행동을 입력해주세요.");
            InputPlayer(minNumber, maxNumber);
            if (player.PInput == 0)
            {
                Console.Clear();
                StartMenu();
            }
        }
        
        
        
        //인벤토리
        private static void Inventory()
        {
            minNumber = 0; maxNumber = 1;
            Console.WriteLine("인벤토리\n보유 중인 아이템을 관리할 수 있습니다.\n");
            //아이템 목록나열
            Console.WriteLine("[아이템 목록]");
            //아이템 나열
            for (int i = 0; i < Item.ItemCount; i++)
            {
                if (items[i].IsPurchased)
                {
                    items[i].ItemDescription(i + 1, false, false);//번호 X ,가격보기 X
                }

            }
            Console.WriteLine("\n1. 장착 관리\n0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요");
            InputPlayer(minNumber, maxNumber);
            if (player.PInput == 0)
            {
                Console.Clear();
                StartMenu();
            }
            else if (player.PInput == 1)
            {
                Console.Clear();
                EquipMenu();
            }


        }
        //장착관리
        private static void EquipMenu()
        {
            minNumber = 0;
            
            string equipText = "";

            while (true)
            {
                maxNumber = 0;
                Console.WriteLine("인벤토리 - 장착 관리\n보유 중인 아이템을 장착/해제 할 수 있습니다.\n");
                //아이템 목록나열
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < Item.ItemCount; i++)
                {
                    if (items[i].IsPurchased)
                    {
                        maxNumber++;
                        items[i].ItemDescription(maxNumber, true, false);//번호 O ,가격보기 X
                        items[i].equIpcount = maxNumber;
                    }
                }
                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.WriteLine(equipText);
                equipText = "";
                InputPlayer(minNumber, maxNumber);
                if (player.PInput == 0)
                {
                    Console.Clear();
                    Inventory();
                }
                else
                {
                    for(int i = 0;i < Item.ItemCount;i++)
                    {   
                        if (player.PInput == items[i].equIpcount)
                        {
                            //아이템 장착(상태창에서 스텟 적용)
                            if (!items[i].IsEquipped && items[i].IsPurchased)
                            {
                                equipText = "아이템을 장착했습니다.";
                                items[i].IsEquipped = true;
                                if (items[i].Type == 0)//방어력 스텟
                                {
                                    player.PDef += items[i].DEF;
                                }
                                else if (items[i].Type == 1)//공격력 스텟
                                {
                                    player.PAtk += items[i].ATK;
                                }
                            }
                            //아이템 장착 해제(상태창에서 스텟 제거)
                            else if (items[i].IsEquipped)
                            {
                                equipText = "아이템 장착을 해제했습니다.";
                                items[i].IsEquipped = false;
                                if (items[i].Type == 0)//방어력 스텟
                                {
                                    player.PDef -= items[i].DEF;
                                }
                                else if (items[i].Type == 1)//공격력 스텟
                                {
                                    player.PAtk -= items[i].ATK;
                                }
                            }
                        }
                    }
                    

                }
                Console.Clear();
            }
        }

        //상점
        private static void Shop()
        {
            minNumber = 0; maxNumber = 1;
            Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine($"[보유 골드]\n{player.PGold} G\n");
            //아이템 목록나열
            Console.WriteLine("[아이템 목록]");

            //아이템 나열
            for (int i = 0; i < Item.ItemCount; i++)
            {
                items[i].ItemDescription(i + 1, false, true);//번호 X ,가격보기 O
            }
            Console.WriteLine("\n1. 아이템 구매\n0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            InputPlayer(minNumber, maxNumber);
            if (player.PInput == 0)
            {
                Console.Clear();
                StartMenu();
            }
            else if (player.PInput == 1)
            {
                Console.Clear();
                BuyItem();
            }
        }
        //아이템 구매
        private static void BuyItem()
        {
            minNumber = 0; maxNumber = 6;
            string purchasText = "";
            while (true) 
            {
                Console.WriteLine("상점 - 아이템 구매\n필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine($"[보유 골드]\n{player.PGold} G\n");
                //아이템 목록나열
                Console.WriteLine("[아이템 목록]");
                //아이템 나열
                for (int i = 0; i < Item.ItemCount; i++)
                {
                    items[i].ItemDescription(i + 1, true, true);//번호 O, 가격보기 O
                }
                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.WriteLine(purchasText);
                purchasText = "";

                InputPlayer(minNumber, maxNumber);
                if (player.PInput == 0)
                {
                    Console.Clear();
                    Shop();
                }
                else
                {
                    if(player.PGold >= items[player.PInput - 1].Gold && !items[player.PInput - 1].IsPurchased)
                    {
                        items[player.PInput - 1].IsPurchased = true;
                        player.PGold -= items[player.PInput - 1].Gold;
                        purchasText = "구매를 완료했습니다.";
                    }
                    else if (items[player.PInput - 1].IsPurchased)
                    {
                        purchasText = "이미 구매한 아이템입니다";
                    }
                    else if (player.PGold < items[player.PInput - 1].Gold)
                    {
                        purchasText = "Gold가 부족합니다.";
                    }
                    Console.Clear();
                }
            }
        }

        //게임 시작전 아이템 정보 저장
        private static void DataSetting()
        {
            items = new Item[6];//아이템을 배열에 저장

            //이름,타입,설명, 공격력,방어력,골드
            AddItem(new Item("무쇠 갑옷", 0, "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 7, 500));
            AddItem(new Item("수련자 갑옷", 0, "수련에 도움을 주는 갑옷입니다.", 0, 5, 1000));
            AddItem(new Item("스파르타의 갑옷", 0, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 0, 15, 2500));
            AddItem(new Item("낡은 검", 1, "쉽게 볼 수 있는 낡은 검 입니다.", 2, 0, 600));
            AddItem(new Item("청동 도끼", 1, "어디선가 사용됐던거 같은 도끼입니다.", 5, 0, 1500));
            AddItem(new Item("스파르타의 창", 1, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 7, 0, 3500));
        }

        private static void AddItem(Item item)
        {
            if (Item.ItemCount == 6)
                return;

            items[Item.ItemCount] = item;
            Item.ItemCount++;
        }

        //입력확인
        private static int InputPlayer(int minNumber,int maxNumber)
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out player.PInput)&&    //int.TryParse를 이용해 참,거짓을 받고
                   (player.PInput >= minNumber&& player.PInput <= maxNumber))//상호작용하는 번호의 최소와 최대와 비교를해 실행
                {
                    return player.PInput;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                }
            }
        }
    }

    
    
}
