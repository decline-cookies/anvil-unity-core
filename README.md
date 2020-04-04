# anvil-unity-core

Content Manager Quick Start


Create a new instance of a Content Manager

ex: m_UIContentManager = new ContentManager("UI", UIRoot);

Takes in an ID for referencing later and a GameObject transform that will be the Root GameObject. You get access to this however you want. GameObject.FindWithTag or direct reference on a prefab, whatever.

Next configure layers as you see fit. 

ex. m_UIContentManager.CreateContentLayer(ContentLayerConfigVO.Create("VIEW", new Vector3(0.0f, 0.0f, 0.0f)))
                	  .CreateContentLayer(ContentLayerConfigVO.Create("MODAL", new Vector3 (0.0f, 0.0f, -10.0f)));

Layers have an ID that will show up in the GameObject hierarchy at the specified vector offset as a child of the Root GameObject you made your Content Manager with. 

Then show your content:

ex: m_UIContentManager.Show(new ViewMainController());

Your Controller will extend AbstractContentController which will make you implement a bunch of abstract methods. Should look something like this:

public class ViewMainController : AbstractContentController
{
    public ViewMainController()
    {
    }

    protected override void DisposeSelf()
    {
        base.DisposeSelf();
    }

    protected override void InitConfigVO(ContentControllerConfigVO configVO)
    {
        configVO.ContentLayerID = "VIEW";
        configVO.ContentLoadingID = "Content/View/ViewMain";
    }

    protected override void InitAfterLoadComplete()
    {
       
    }

    protected override void PlayIn()
    {
        PlayInComplete();
    }

    protected override void InitAfterPlayInComplete()
    {
        
    }

    protected override void PlayOut()
    {
        PlayOutComplete();
    }
}

All content is assumed to be a prefab in Resources right now. 

Lots of TODO's to change in Issues and cleanup but it and all the public API's work today.