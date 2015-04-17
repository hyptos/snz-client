using UnityEngine;
using System.Collections;
using System.Collections.Generic ; 

public class IHM : MonoBehaviour 
{
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
	int pts_energie_max = 70 ; 
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
	int pts_exp_joueur = 50 ; 
	int pts_energie_joueur = 10 ; 
	int nb_comp_joueur = 2 ; 
	int nb_armes_joueur = 1 ; 
	int nb_nourriture_joueur = 2 ; 
	int nb_objets_joueur = 0 ; 
	int nb_amis_joueur = 3 ; 
	List<string> amis_joueur = new List<string> {"Christophe", "Toto", "Titi" } ; 
	List<string> competences_joueur = new List<string> { "Attaquer", "Parer" };
	List<string> armes_joueur = new List<string> { "Couteau" };
	List<string> nourritures_joueur = new List<string> { "Pomme", "Eau" }; 
	List<string> objets_joueur = new List<string>() ;

	/*******************************************/
	/*** Paramètres de l'interface *************/
	/*******************************************/
	// Champs du chat
	string champ_chat = "" ;
	string discussion = "" ;
	// Ouverture des fenetres invisibles
	bool enterOk = false ; 
	bool inventaireOuvert = false ; 
	bool quitterOuvert = false ; 
	bool paramsOuvert = false ; 
	bool socialOuvert = false ; 
	// Styles
	GUIStyle styleBouton ; 
	GUIStyle styleLabel ; 
	GUIStyle styleBarreFond = null;
	GUIStyle styleBarreManque = null;
	GUIStyle styleBarreVie = null;
	GUIStyle styleBarreEnergie = null;
	GUIStyle styleBarreExperience = null;
	GUIStyle styleFondInvisible = null;

	void OnGUI()
	{
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
		// Barre vide
		styleBarreManque = new GUIStyle( GUI.skin.box );
		styleBarreManque.normal.background = MakeTex( 2, 2, new Color( 0.9f, 0.9f, 0.9f, 1f ) );
		// Barre de vie
		styleBarreVie = new GUIStyle( GUI.skin.box );
		styleBarreVie.normal.background = MakeTex( 2, 2, new Color( 0.25f, 0.7f, 0.25f, 1f ) );
		// Barre d'énergie
		styleBarreEnergie = new GUIStyle( GUI.skin.box );
		styleBarreEnergie.normal.background = MakeTex( 2, 2, new Color( 1f, 0f, 1f, 0.8f ) );
		// Barre d'expérience
		styleBarreExperience = new GUIStyle( GUI.skin.box );
		styleBarreExperience.normal.background = MakeTex( 2, 2, new Color( 1f, 0.92f, 0.1f, 0.8f ) );


		//********** Barre de vie et image du personnage en haut à gauche **********
		float hauteur_perso = (float) 0.12 * Screen.height; 
		float largeur_perso = (float) 0.07 * Screen.width; 
		float hauteur_barres = (float) 0.20 * hauteur_perso ; 
		float largeur_barres = (float) 0.18 * Screen.width ; 
		GUI.Box (new Rect (10, 10, largeur_perso, hauteur_perso), Resources.Load(nom_image_joueur) as Texture, styleBouton);
		GUI.Box (new Rect (10 + largeur_perso, 10, largeur_barres+4, hauteur_barres*3+4+4), "", styleBarreFond);
		//GUI.Box (new Rect (10, 10 + hauteur_perso - 32, largeur_perso, 32), pseudo_joueur); 
		// Vie
		float vie = (float) pts_vie_joueur / pts_vie_max ; 
		GUI.Box (new Rect (12 + largeur_perso, 10+2, largeur_barres, hauteur_barres), "", styleBarreManque);
		GUI.Box (new Rect (12 + largeur_perso, 10+2, vie*largeur_barres, hauteur_barres), "", styleBarreVie);
		// Energie
		float en = (float) pts_energie_joueur / pts_energie_max ; 
		GUI.Box (new Rect (12 + largeur_perso, 10+2+2 + hauteur_barres, largeur_barres, hauteur_barres), "", styleBarreManque); 
		GUI.Box (new Rect (12 + largeur_perso, 10+2+2 + hauteur_barres, en*largeur_barres, hauteur_barres), "", styleBarreEnergie); 
		// Expérience
		float exp = (float) pts_exp_joueur / pts_exp_max ; 
		GUI.Box (new Rect (12 + largeur_perso, 10+2+4 + hauteur_barres*2, largeur_barres, hauteur_barres), "", styleBarreManque); 
		GUI.Box (new Rect (12 + largeur_perso, 10+2+4 + hauteur_barres*2, exp*largeur_barres, hauteur_barres), "", styleBarreExperience); 


		//********** Chat **********
		float hauteur = (float) 0.30 * Screen.height; 
		float largeur = (float) 0.27 * Screen.width; 
		GUI.Box (new Rect (10,Screen.height-hauteur,largeur,hauteur-10), "");
		// Champs
		GUI.SetNextControlName ("chatWindow"); 
		
		GUI.TextArea (new Rect (10,Screen.height-hauteur,largeur,hauteur-32), discussion);
		Event e = Event.current;
		if (e.type == EventType.keyDown && e.keyCode == KeyCode.Return && champ_chat != "") {
			discussion = discussion + "\n" + pseudo_joueur + " : " + champ_chat ; 
			champ_chat = "";
		}
		champ_chat = GUI.TextField (new Rect (10,Screen.height-32,largeur-50,32-10), champ_chat,100);
		if (GUI.Button(new Rect (10+largeur-50,Screen.height-32,50,32-10), "Send") && champ_chat != "")
		{
			//networkView.RPC("SendMessage", RPCMode.All, champ_chat + "\n");
			discussion = discussion + "\n" + pseudo_joueur + " : " + champ_chat ; 
			champ_chat = "";
		}


		//********** Compétences **********
		float hauteur_comp = (float) 0.28 * hauteur; 
		float largeur_comp = (float) 0.4 * Screen.width; 
		float marge_comp_g = (float) 0.05 * Screen.width; 
		GUI.Box (new Rect (largeur + marge_comp_g, Screen.height - hauteur_comp, largeur_comp, hauteur_comp - 10), "", styleFondInvisible);
		// Cases
		float largeur_case_comp = (float)0.1 * largeur_comp; 
		int i = 0; 
		for (i = 0; i < nb_comp_joueur ; i++) 
		{
			GUI.Button(new Rect (largeur + marge_comp_g + i*largeur_case_comp, Screen.height - hauteur_comp, largeur_case_comp, hauteur_comp - 10), Resources.Load(competences_joueur[i]) as Texture, styleBouton) ; 
		}


		//********** Menu **********
		float hauteur_menu = (float) 0.2 * hauteur; 
		float largeur_menu = (float) 0.10 * Screen.width; 
		GUI.Box (new Rect (Screen.width - largeur_menu - 10, Screen.height - hauteur_menu, largeur_menu, hauteur_menu - 10), "");
		// Cases
		float largeur_case_menu = (float)0.25 * largeur_menu; 
		if (GUI.Button (new Rect (Screen.width - largeur_menu - 10 + 0 * largeur_case_menu, Screen.height - hauteur_menu, largeur_case_menu, hauteur_menu - 10), Resources.Load ("social") as Texture, styleBouton)) {
			paramsOuvert = false; 
			quitterOuvert = false; 
			inventaireOuvert = false; 
			socialOuvert = ! socialOuvert; 
		}
		if (GUI.Button (new Rect (Screen.width - largeur_menu - 10 + 1 * largeur_case_menu, Screen.height - hauteur_menu, largeur_case_menu, hauteur_menu - 10), Resources.Load ("sac") as Texture, styleBouton)) {
			quitterOuvert = false ; 
			paramsOuvert = false ; 
			socialOuvert = false ; 
			inventaireOuvert = ! inventaireOuvert ; 
		}
		if (GUI.Button (new Rect (Screen.width - largeur_menu - 10 + 2 * largeur_case_menu, Screen.height - hauteur_menu, largeur_case_menu, hauteur_menu - 10), Resources.Load ("params") as Texture, styleBouton)) {
			quitterOuvert = false; 
			inventaireOuvert = false; 
			socialOuvert = false; 
			paramsOuvert = ! paramsOuvert; 
		}
		if (GUI.Button (new Rect (Screen.width - largeur_menu - 10 + 3 * largeur_case_menu, Screen.height - hauteur_menu, largeur_case_menu, hauteur_menu - 10), Resources.Load ("power") as Texture, styleBouton)) {
			inventaireOuvert = false ; 
			paramsOuvert = false ; 
			socialOuvert = false ; 
			quitterOuvert = !quitterOuvert ; 
		}


		//********** Map **********
		float hauteur_map = (float) 0.25 * Screen.height; 
		GUI.Box (new Rect (Screen.width - hauteur_map - 10, 10, hauteur_map, hauteur_map), "");


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
			float hauteur_descr = (float) (hauteur_inventaire/3)/4 ; 
			GUI.Label(new Rect(x_inventaire+largeur_inventaire/3+5, y_inventaire, largeur_inventaire*2/3, hauteur_descr), pseudo_joueur) ; 
			GUI.Label(new Rect(x_inventaire+largeur_inventaire/3+5, y_inventaire+hauteur_descr, largeur_inventaire*2/3, hauteur_descr), "Vie : "+pts_vie_joueur+"/"+pts_vie_max) ; 
			GUI.Label(new Rect(x_inventaire+largeur_inventaire/3+5, y_inventaire+2*hauteur_descr, largeur_inventaire*2/3, hauteur_descr), "Expérience : "+pts_exp_joueur+"/"+pts_exp_max) ; 
			GUI.Label(new Rect(x_inventaire+largeur_inventaire/3+5, y_inventaire+3*hauteur_descr, largeur_inventaire*2/3, hauteur_descr), "Energie : "+pts_energie_joueur+"/"+pts_energie_max) ; 
			// --- Box sac
			GUI.Box (new Rect(x_inventaire, y_inventaire+hauteur_inventaire/3, largeur_inventaire/3, hauteur_inventaire*2/3), "") ;
			GUI.Box (new Rect(x_inventaire+largeur_inventaire/3, y_inventaire+hauteur_inventaire/3, largeur_inventaire/3, hauteur_inventaire*2/3), "") ;
			GUI.Box (new Rect(x_inventaire+2*largeur_inventaire/3, y_inventaire+hauteur_inventaire/3, largeur_inventaire/3, hauteur_inventaire*2/3), "") ;
			// --- --- Labels 
			GUI.Label(new Rect(x_inventaire, y_inventaire+hauteur_inventaire/3+5, largeur_inventaire/3, 32), "Armes", styleLabel) ; 
			GUI.Label(new Rect(x_inventaire+largeur_inventaire/3, y_inventaire+hauteur_inventaire/3+5, largeur_inventaire/3, 32), "Nourriture", styleLabel) ; 
			GUI.Label(new Rect(x_inventaire+2*largeur_inventaire/3, y_inventaire+hauteur_inventaire/3+5, largeur_inventaire/3, 32), "Objets spéciaux", styleLabel) ; 
			//--- --- Objets 
			float largeur_case = (float) ((largeur_inventaire/3)-10)/4; // 4 colonnes
			float hauteur_case = (float) (2*hauteur_inventaire/3-5-32)/6 ; 
			for(i = 0 ; i < 24 ; i++)
			{
				int colonne = (int) i/4 ; 
				// Armes
				if(i < nb_armes_joueur)
				{
					GUI.Button(new Rect (x_inventaire+(i%4)*largeur_case+5, y_inventaire+hauteur_inventaire/3+5+32+hauteur_case*colonne, largeur_case, hauteur_case), ""+armes_joueur[i]) ; 
				}
				// Nourriture
				if(i < nb_nourriture_joueur)
				{
					GUI.Button(new Rect (x_inventaire+(i%4)*largeur_case+largeur_case*4+15, y_inventaire+hauteur_inventaire/3+5+32+hauteur_case*colonne, largeur_case, hauteur_case), ""+nourritures_joueur[i]) ; 
				}
				// Objets spéciaux
				if(i < nb_objets_joueur)
				{
					GUI.Button(new Rect (x_inventaire+(i%4)*largeur_case+largeur_case*8+25, y_inventaire+hauteur_inventaire/3+5+32+hauteur_case*colonne, largeur_case, hauteur_case), ""+objets_joueur[i]) ; 
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
			GUI.Button (new Rect(x_quitter+10, y_quitter + hauteur_quitter/2, largeur_quitter/2-15, 48), "Oui") ;  
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
			GUI.Button (new Rect(x_params+10, y_params +10, largeur_params-20, 48), "Activer/Desactiver musique") ;  
			GUI.Button (new Rect(x_params+10, y_params +20+48, largeur_params-20, 48), "Activer/Desactiver son") ;
			GUI.Label (new Rect(x_params+10, y_params+30+48*2, largeur_params-20, 32), "Social : O") ; 
			GUI.Label (new Rect(x_params+10, y_params+40+48*2+32*1, largeur_params-20, 32), "Inventaire : I") ; 
			GUI.Label (new Rect(x_params+10, y_params+50+48*2+32*2, largeur_params-20, 32), "Paramètres : P") ; 
			GUI.Label (new Rect(x_params+10, y_params+60+48*2+32*3, largeur_params-20, 32), "Quitter : Echap") ; 
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
			for(i = 0 ; i < 10 ; i++)
			{
				if(i < nb_amis_joueur)
				{
					GUI.Box(new Rect(x_social,y_social+i*hauteur_social/10, largeur_social, hauteur_social/10), amis_joueur[i]) ; 
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

	void update()
	{
		if (Input.GetKey(KeyCode.KeypadEnter)) 
		{
			enterOk = true; 
		} 
		else 
		{
			enterOk = false ; 
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
}
