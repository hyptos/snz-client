using UnityEngine;
using System.Collections;
using System.Collections.Generic ; 
using System.Threading;

public class IHM : MonoBehaviour 
{
	/*******************************************/
	/*** Paramètres d'interface ****************/
	/*******************************************/
	int MARGE_GENERALE = 10 ;  
	int TAILLE_TEXTE = 32 ;  
	
	
	/*******************************************/
	/*** Paramètres généraux du jeu ************/
	/*******************************************/
	// On suppose que les 3 paramètres suivant ne dépassent pas 24
	int nb_armes_jeu = 3 ; 
	int nb_nourriture_jeu = 5 ; 
	int nb_objets_jeu = 6 ; 
	// On suppose que le nombre de compétences ne dépasse pas 10
	int nb_comp_jeu = 5 ; 
	int pts_vie_max = 100 ; 
	int pts_exp_max = 50 ; 
	static int pts_energie_max = 100 ; 
	List<string> competences = new List<string> { "Attaquer", "Parer", "Esquiver", "Sauter", "Rouler" };
	List<string> armes = new List<string> {"Couteau", "Gourdin", "Pistolet"};
	List<string> nourritures = new List<string> { "Pomme", "Gateaux", "Eau", "Cereales", "Viande" }; 
	List<string> objets = new List<string> { "Carte", "Corde", "Filet", "Boussole", "Potion", "Antidote" };
	
	/*******************************************/
	/*** Données liées au joueur ***************/
	/*******************************************/
	string pseudo_joueur = "Sacapof" ; 
	string nom_image_joueur = "test_image_joueur" ; 
	int pts_vie_joueur = 70 ; 
	int pts_exp_joueur = 40 ; 
	static int pts_energie_joueur = 10 ; 
	int nb_comp_joueur = 2 ; 
	int nb_armes_joueur = 2 ; 
	int nb_nourriture_joueur = 2 ; 
	int nb_objets_joueur = 0 ; 
	int nb_amis_joueur = 3 ; 
	List<string> amis_joueur = new List<string> {"Christophe", "Toto", "Titi" } ; 
	List<string> competences_joueur = new List<string> { "Attaquer", "Parer" };
	List<string> armes_joueur = new List<string> { "Couteau", "Gourdin" };
	string arme_equipee = "Couteau" ; 
	List<string> nourritures_joueur = new List<string> { "Pomme", "Eau" }; 
	List<int> nourritures_joueur_quantite = new List<int> { 5, 2 }; 
	List<string> objets_joueur = new List<string>() ;
	
	/*******************************************/
	/*** Paramètres de l'interface *************/
	/*******************************************/
	// Champs du chat
	string champ_chat = "" ;
	string discussion = "" ;
	// Ouverture des fenetres invisibles 
	bool inventaireOuvert = false ; 
	bool quitterOuvert = false ; 
	bool paramsOuvert = false ; 
	bool socialOuvert = false ;
	bool chatFocus = false ;
	// Styles
	GUIStyle styleBouton ; 
	GUIStyle styleLabel ; 
	GUIStyle styleBarreFond = null;
	GUIStyle styleFondInvisible = null;
	GUISkin skinBarreVie = null;
	GUISkin skinBarreEnergie = null;
	GUISkin skinBarreExperience = null;
	GUISkin skinBarreVide = null;
	GUISkin skinBasique = null;
	// Treads
	bool threadDemarre = false;
	static Thread thEnergie = new Thread (new ThreadStart (augmenteEnergie));


	void OnGUI()
	{
		if (threadDemarre == false) 
		{
			thEnergie.Start ();
			threadDemarre = true;
		}
		// Barre de vie 
		Object textureBarreVie = Resources.Load ("Vie");
		skinBarreVie = new GUISkin ();
		skinBarreVie.box.normal.background = (Texture2D)textureBarreVie;
		// Barre d'énergie
		Object textureBarreEnergie = Resources.Load ("energie");
		skinBarreEnergie = new GUISkin ();
		skinBarreEnergie.box.normal.background = (Texture2D)textureBarreEnergie;
		// Barre d'exp
		Object textureBarreExp = Resources.Load ("experience");
		skinBarreExperience = new GUISkin ();
		skinBarreExperience.box.normal.background = (Texture2D)textureBarreExp;
		// Barre vide
		Object textureBarreVide = Resources.Load ("manque");
		skinBarreVide = new GUISkin ();
		skinBarreVide.box.normal.background = (Texture2D)textureBarreVide;

		// Boutons 
		styleBouton = new GUIStyle (); 
		styleBouton.stretchWidth = true; 
		styleBouton.stretchHeight = true;
		// Labels
		styleLabel = new GUIStyle (); 
		styleLabel.alignment = TextAnchor.UpperCenter;
		styleLabel.normal.textColor = Color.white; 
		// Fond des barres 
		styleBarreFond = new GUIStyle( GUI.skin.box );
		styleBarreFond.normal.background = MakeTex( 2, 2, new Color( 0f, 0f, 0f, 1f ) );
		// Fond invisible 
		styleFondInvisible = new GUIStyle( GUI.skin.box );
		styleFondInvisible.normal.background = MakeTex( 2, 2, new Color( 0f, 0f, 0f, 0f ) );
		
		
		//********** Barre de vie et image du personnage en haut à gauche **********
		float hauteur_perso = (float) 0.12 * Screen.height; 
		float largeur_perso = (float) 0.07 * Screen.width; 
		float hauteur_barres = (float) 0.2 * hauteur_perso ; 
		float largeur_barres = (float) 0.18 * Screen.width ; 
		//GUI.Box (new Rect (MARGE_GENERALE, MARGE_GENERALE, largeur_perso, hauteur_perso), Resources.Load(nom_image_joueur) as Texture, styleBouton);
		GUI.Box (new Rect (MARGE_GENERALE + largeur_perso, MARGE_GENERALE, largeur_barres+4, hauteur_barres*3+4+4), "", styleBarreFond);

		// Vie
		float vie = (float) pts_vie_joueur / pts_vie_max ; 
		GUI.skin = skinBarreVide;
		GUI.Box (new Rect (12 + largeur_perso, 10+2, largeur_barres, hauteur_barres), "");
		GUI.skin = skinBasique;
		skinBasique = GUI.skin;
		GUI.skin = skinBarreVie;
		GUI.Box (new Rect (12 + largeur_perso, 10 + 2, vie * largeur_barres, hauteur_barres), "");
		GUI.skin = skinBasique;
		// Energie
		float en = (float) pts_energie_joueur / pts_energie_max ; 
		GUI.skin = skinBarreVide;
		GUI.Box (new Rect (12 + largeur_perso, 10+2+2 + hauteur_barres, largeur_barres, hauteur_barres), ""); 
		GUI.skin = skinBarreEnergie;
		GUI.Box (new Rect (12 + largeur_perso, 10+2+2 + hauteur_barres, en*largeur_barres, hauteur_barres), ""); 
		GUI.skin = skinBasique;
		// Expérience
		float exp = (float) pts_exp_joueur / pts_exp_max ; 
		GUI.skin = skinBarreVide;
		GUI.Box (new Rect (12 + largeur_perso, 10+2+4 + hauteur_barres*2, largeur_barres, hauteur_barres), ""); 
		GUI.skin = skinBarreExperience;
		GUI.Box (new Rect (12 + largeur_perso, 10+2+4 + hauteur_barres*2, exp*largeur_barres, hauteur_barres), ""); 
		GUI.skin = skinBasique;
		
		//********** Chat **********
		float hauteur = (float) 0.30 * Screen.height; 
		float largeur = (float) 0.27 * Screen.width; 
		GUI.Box (new Rect (MARGE_GENERALE,Screen.height-hauteur,largeur,hauteur-MARGE_GENERALE), "");
		// Champs
		
		GUI.TextArea (new Rect (MARGE_GENERALE,Screen.height-hauteur,largeur,hauteur-TAILLE_TEXTE), discussion);
		//strFocus = GUI.FocusControl;
		Event e = Event.current;
		if (e.type == EventType.keyDown && e.keyCode == KeyCode.Return) 
		{
			chatFocus = !chatFocus;
			if(champ_chat != "")
			{
				discussion = discussion + "\n" + pseudo_joueur + " : " + champ_chat ; 
				champ_chat = "";
			}
		}
		GUI.SetNextControlName ("champ_chat");
		champ_chat = GUI.TextField (new Rect (MARGE_GENERALE,Screen.height-TAILLE_TEXTE,largeur-50,TAILLE_TEXTE-MARGE_GENERALE), champ_chat,100);
		if(chatFocus)
		{
			GUI.FocusControl("champ_chat");
		}
		else
		{
			GUI.FocusControl("");
		}
		if (GUI.Button(new Rect (MARGE_GENERALE+largeur-50,Screen.height-TAILLE_TEXTE,50,TAILLE_TEXTE-MARGE_GENERALE), "Send") && champ_chat != "")
		{
			//networkView.RPC("SendMessage", RPCMode.All, champ_chat + "\n");
			discussion = discussion + "\n" + pseudo_joueur + " : " + champ_chat ; 
			champ_chat = "";
		}
		
		
		//********** Compétences **********
		float hauteur_comp = (float) 0.28 * hauteur; 
		float largeur_comp = (float) 0.4 * Screen.width; 
		float marge_comp_g = (float) 0.05 * Screen.width; 
		GUI.Box (new Rect (largeur + marge_comp_g, Screen.height - hauteur_comp, largeur_comp, hauteur_comp - MARGE_GENERALE), "", styleFondInvisible);
		// Cases
		float largeur_case_comp = (float)0.1 * largeur_comp; 
		int i = 0; 
		for (i = 0; i < nb_comp_joueur ; i++) 
		{
			if(GUI.Button(new Rect (largeur + marge_comp_g + i*largeur_case_comp, Screen.height - hauteur_comp, largeur_case_comp, hauteur_comp - MARGE_GENERALE), Resources.Load(competences_joueur[i]) as Texture, styleBouton))
			{
				baisserEnergie("toto",35);
			}
		}
		
		
		//********** Menu **********
		float hauteur_menu = (float) 0.2 * hauteur; 
		float largeur_menu = (float) 0.1 * Screen.width; 
		GUI.Box (new Rect (Screen.width - largeur_menu - MARGE_GENERALE, Screen.height - hauteur_menu, largeur_menu, hauteur_menu - MARGE_GENERALE), "");
		// Cases
		float largeur_case_menu = (float)0.25 * largeur_menu; 
		if (GUI.Button (new Rect (Screen.width - largeur_menu - MARGE_GENERALE + 0 * largeur_case_menu, Screen.height - hauteur_menu, largeur_case_menu, hauteur_menu - MARGE_GENERALE), Resources.Load ("social") as Texture, styleBouton)) {
			paramsOuvert = false; 
			quitterOuvert = false; 
			inventaireOuvert = false; 
			socialOuvert = ! socialOuvert; 
		}
		if (GUI.Button (new Rect (Screen.width - largeur_menu - MARGE_GENERALE + 1 * largeur_case_menu, Screen.height - hauteur_menu, largeur_case_menu, hauteur_menu - MARGE_GENERALE), Resources.Load ("sac") as Texture, styleBouton)) {
			quitterOuvert = false ; 
			paramsOuvert = false ; 
			socialOuvert = false ; 
			inventaireOuvert = ! inventaireOuvert ; 
		}
		if (GUI.Button (new Rect (Screen.width - largeur_menu - MARGE_GENERALE + 2 * largeur_case_menu, Screen.height - hauteur_menu, largeur_case_menu, hauteur_menu - MARGE_GENERALE), Resources.Load ("params") as Texture, styleBouton)) {
			quitterOuvert = false; 
			inventaireOuvert = false; 
			socialOuvert = false; 
			paramsOuvert = ! paramsOuvert; 
		}
		if (GUI.Button (new Rect (Screen.width - largeur_menu - MARGE_GENERALE + 3 * largeur_case_menu, Screen.height - hauteur_menu, largeur_case_menu, hauteur_menu - MARGE_GENERALE), Resources.Load ("power") as Texture, styleBouton)) {
			inventaireOuvert = false ; 
			paramsOuvert = false ; 
			socialOuvert = false ; 
			quitterOuvert = !quitterOuvert ; 
		}
		
		
		//********** Map **********
		//float largeur_map = (float)0.18 * Screen.width; 
		//float hauteur_map = (float) 0.25 * Screen.height; 
		//GUI.Box (new Rect (Screen.width - largeur_map - MARGE_GENERALE, MARGE_GENERALE, largeur_map, hauteur_map), "");

		
		//********** Inventaire **********
		if (inventaireOuvert) 
		{
			float largeur_inventaire = (float) 0.4 * Screen.width ; 
			float hauteur_inventaire = (float) 0.6 * Screen.height ; 
			float x_inventaire = (float) (Screen.width - largeur_inventaire) / 2 ;
			float y_inventaire = (float) (Screen.height - largeur_inventaire) / 2 ;
			
			// --- Box photo personnage
			GUI.Box (new Rect(x_inventaire, y_inventaire, largeur_inventaire/3, hauteur_inventaire/3), Resources.Load(nom_image_joueur) as Texture, styleBouton) ; 
			// --- Box caractéristiques 
			GUI.Box (new Rect(x_inventaire+largeur_inventaire/3, y_inventaire, largeur_inventaire*2/3, hauteur_inventaire/3), "") ; 
			// --- --- Description du personnage
			float hauteur_descr = (float) (hauteur_inventaire/3)/5 ; 
			GUI.Label(new Rect(x_inventaire+largeur_inventaire/3+5, y_inventaire, largeur_inventaire*2/3, hauteur_descr), pseudo_joueur) ; 
			GUI.Label(new Rect(x_inventaire+largeur_inventaire/3+5, y_inventaire+hauteur_descr, largeur_inventaire*2/3, hauteur_descr), "Vie : "+pts_vie_joueur+"/"+pts_vie_max) ; 
			GUI.Label(new Rect(x_inventaire+largeur_inventaire/3+5, y_inventaire+2*hauteur_descr, largeur_inventaire*2/3, hauteur_descr), "Expérience : "+pts_exp_joueur+"/"+pts_exp_max) ; 
			GUI.Label(new Rect(x_inventaire+largeur_inventaire/3+5, y_inventaire+3*hauteur_descr, largeur_inventaire*2/3, hauteur_descr), "Energie : "+pts_energie_joueur+"/"+pts_energie_max) ; 
			GUI.Label(new Rect(x_inventaire+largeur_inventaire/3+5, y_inventaire+4*hauteur_descr, largeur_inventaire*2/3, hauteur_descr), "Arme équipée : "+arme_equipee) ; 
			// --- Box sac
			GUI.Box (new Rect(x_inventaire, y_inventaire+hauteur_inventaire/3, largeur_inventaire/3, hauteur_inventaire*2/3), "") ;
			GUI.Box (new Rect(x_inventaire+largeur_inventaire/3, y_inventaire+hauteur_inventaire/3, largeur_inventaire/3, hauteur_inventaire*2/3), "") ;
			GUI.Box (new Rect(x_inventaire+2*largeur_inventaire/3, y_inventaire+hauteur_inventaire/3, largeur_inventaire/3, hauteur_inventaire*2/3), "") ;
			// --- --- Labels 
			GUI.Label(new Rect(x_inventaire, y_inventaire+hauteur_inventaire/3+5, largeur_inventaire/3, TAILLE_TEXTE), "Armes", styleLabel) ; 
			GUI.Label(new Rect(x_inventaire+largeur_inventaire/3, y_inventaire+hauteur_inventaire/3+5, largeur_inventaire/3, TAILLE_TEXTE), "Nourriture", styleLabel) ; 
			GUI.Label(new Rect(x_inventaire+2*largeur_inventaire/3, y_inventaire+hauteur_inventaire/3+5, largeur_inventaire/3, TAILLE_TEXTE), "Objets spéciaux", styleLabel) ; 
			//--- --- Objets 
			float largeur_case = (float) ((largeur_inventaire/3)-MARGE_GENERALE)/4; // 4 colonnes
			float hauteur_case = (float) (2*hauteur_inventaire/3-5-TAILLE_TEXTE)/6 ; 
			for(i = 0 ; i < 24 ; i++)
			{
				int colonne = (int) i/4 ; 
				// Armes
				if(i < nb_armes_joueur)
				{
					if(GUI.Button(new Rect (x_inventaire+(i%4)*largeur_case+5, y_inventaire+hauteur_inventaire/3+5+TAILLE_TEXTE+hauteur_case*colonne, largeur_case, hauteur_case), ""+armes_joueur[i]))
					{
						changeArmeEquipee(armes_joueur[i]) ; 
					}
				}
				// Nourriture
				if(i < nb_nourriture_joueur)
				{
					if(GUI.Button(new Rect (x_inventaire+(i%4)*largeur_case+largeur_case*4+15, y_inventaire+hauteur_inventaire/3+5+TAILLE_TEXTE+hauteur_case*colonne, largeur_case, hauteur_case), ""+nourritures_joueur[i]))
					{
						augmenteVie(nourritures_joueur[i], i) ; 
					}
				}
				// Objets spéciaux
				if(i < nb_objets_joueur)
				{
					GUI.Button(new Rect (x_inventaire+(i%4)*largeur_case+largeur_case*8+25, y_inventaire+hauteur_inventaire/3+5+TAILLE_TEXTE+hauteur_case*colonne, largeur_case, hauteur_case), ""+objets_joueur[i]) ; 
				}
			}
		}
		
		
		//********** Quitter **********
		if (quitterOuvert) 
		{
			float largeur_quitter = (float) 0.2 * Screen.width ; 
			float hauteur_quitter = (float) 0.3 * Screen.height ; 
			float x_quitter = (float) (Screen.width - largeur_quitter) / 2 ;
			float y_quitter = (float) (Screen.height - hauteur_quitter) / 2 ;
			
			// --- Box photo personnage
			GUI.Box (new Rect(x_quitter, y_quitter, largeur_quitter, hauteur_quitter), "Vous etes sur le point de quitter. \nContinuer ?") ; 
			GUI.Button (new Rect(x_quitter+MARGE_GENERALE, y_quitter + hauteur_quitter/2, largeur_quitter/2-15, 48), "Oui") ;  
			if(GUI.Button (new Rect(x_quitter+5+largeur_quitter/2, y_quitter + hauteur_quitter/2, largeur_quitter/2-15, 48), "Non"))
			{
				quitterOuvert = false ; 
			}
		}
		
		
		//********** Paramètres **********
		if (paramsOuvert) 
		{
			float largeur_params = (float) 0.2 * Screen.width ; 
			float hauteur_params = (float) 0.4 * Screen.height ; 
			float x_params = (float) (Screen.width - largeur_params) / 2 ;
			float y_params = (float) (Screen.height - hauteur_params) / 2 ;
			
			// --- Box photo personnage
			GUI.Box (new Rect(x_params, y_params, largeur_params, hauteur_params), "") ; 
			GUI.Button (new Rect(x_params+MARGE_GENERALE, y_params +MARGE_GENERALE, largeur_params-20, 48), "Activer/Desactiver musique") ;  
			GUI.Button (new Rect(x_params+MARGE_GENERALE, y_params +20+48, largeur_params-20, 48), "Activer/Desactiver son") ;
			GUI.Label (new Rect(x_params+MARGE_GENERALE, y_params+30+48*2, largeur_params-20, TAILLE_TEXTE), "Social : O") ; 
			GUI.Label (new Rect(x_params+MARGE_GENERALE, y_params+40+48*2+TAILLE_TEXTE*1, largeur_params-20, TAILLE_TEXTE), "Inventaire : I") ; 
			GUI.Label (new Rect(x_params+MARGE_GENERALE, y_params+50+48*2+TAILLE_TEXTE*2, largeur_params-20, TAILLE_TEXTE), "Paramètres : P") ; 
			GUI.Label (new Rect(x_params+MARGE_GENERALE, y_params+60+48*2+TAILLE_TEXTE*3, largeur_params-20, TAILLE_TEXTE), "Quitter : Echap") ; 
		}
		
		
		//********** Social **********
		if (socialOuvert) 
		{
			float largeur_social = (float) 0.32 * Screen.width ; 
			float hauteur_social = (float) 0.4 * Screen.height ; 
			float x_social = (float) (Screen.width - largeur_social) / 2 ;
			float y_social = (float) (Screen.height - hauteur_social) / 2 ;
			
			// --- Box photo personnage
			GUI.Box (new Rect(x_social, y_social, largeur_social, hauteur_social), "") ;  
			for(i = 0 ; i < MARGE_GENERALE ; i++)
			{
				if(i < nb_amis_joueur)
				{
					GUI.Box(new Rect(x_social,y_social+i*hauteur_social/MARGE_GENERALE, largeur_social, hauteur_social/MARGE_GENERALE), amis_joueur[i]) ; 
				}
			}
		}
	}
	
	void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.I)) 
		{
			socialOuvert = false ; 
			paramsOuvert = false ; 
			quitterOuvert = false ; 
			inventaireOuvert = ! inventaireOuvert ; 
		}
		else if(Input.GetKey (KeyCode.Escape))
		{
			socialOuvert = false ; 
			paramsOuvert = false ;  
			inventaireOuvert = false ; 
			quitterOuvert = ! quitterOuvert ; 
		}
		else if(Input.GetKey (KeyCode.P))
		{
			socialOuvert = false ; 
			quitterOuvert = false ; 
			inventaireOuvert = false ; 
			paramsOuvert = ! paramsOuvert ; 
		}
		else if(Input.GetKey (KeyCode.O))
		{
			paramsOuvert = false ; 
			quitterOuvert = false ; 
			inventaireOuvert = false ; 
			socialOuvert = ! socialOuvert ; 
		}
	}
	
	Texture2D MakeTex( int width, int height, Color col )
	{
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i )
		{
			pix[ i ] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}
	
	void changeArmeEquipee(string arme)
	{
		arme_equipee = arme ; 
	}

	void augmenteVie(string nourriture, int i)
	{
		if (pts_vie_joueur < pts_vie_max && nourritures_joueur_quantite[i] > 0) 
		{
			pts_vie_joueur = pts_vie_joueur + 10  ; 
			if(pts_vie_joueur > pts_vie_max)
			{
				pts_vie_joueur = pts_vie_max ; 
			}

			nourritures_joueur_quantite[i]-- ; 
		}
	}
	
	static void augmenteEnergie() 
	{
		while (thEnergie.IsAlive) {
			if (pts_energie_joueur < pts_energie_max) {
				pts_energie_joueur ++; 
			}
			Thread.Sleep(50) ; 
		}
	}

	void baisserEnergie(string arme, int perte)
	{
		if (pts_energie_joueur > perte) 
		{
			pts_energie_joueur = pts_energie_joueur - perte ;
		}
	}
}
