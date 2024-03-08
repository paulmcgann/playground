define([ // eslint-disable-line
    'dojo/query', // for querying dom elements in widget
    'dojo/on', // event handling
    'dojo/_base/declare',
    'dojo/_base/lang',
    'dijit/_CssStateMixin',
    'dijit/_Widget',
    'dijit/_TemplatedMixin',
    'dijit/_WidgetsInTemplateMixin',
    'dijit/Dialog',
    'epi/epi',
    'epi/shell/widget/_ValueRequiredMixin', // provides validation for required
    "dojo/text!./templates/iconPicker.html",
],
    function (
        query,
        on,
        declare,
        lang,
        _CssStateMixin,
        _Widget,
        _TemplatedMixin,
        _WidgetsInTemplateMixin,
        Dialog,
        epi,
        _ValueRequiredMixin,
        iconPickerTemplate
    ) {
        return declare('foundation.editors.IconPicker',
            [_Widget, _TemplatedMixin, _WidgetsInTemplateMixin, _CssStateMixin, _ValueRequiredMixin],
            {
                // user defined variables...
                SOLID: true, // Change dependent of icon styles loaded in css
                REGULAR: true, // Change dependent of icon styles loaded in css
                BRANDS: false, // Change dependent of icon styles loaded in css
                // non user stuff.
                intermediateChanges: false,
                dialog: null,
                value: null,
                pickedIcon: null,
                templateString: iconPickerTemplate,
                // Add the following code blocks in order here.

                postCreate() {
                    // call the base implementation
                    this.inherited(arguments)

                    // add css files to style our widget, and to load the font awesome icons in edit mode
                    this.addCssLink(`/ClientResources/Scripts/Editors/styles.css`, `interal`)
                    this.addCssLink(`https://pro.fontawesome.com/releases/v5.13.0/css/all.css`, `external`)

                    // create our dialog
                    this.createIconDialog()

                    // do we already have an icon set? In that case, show it!
                    this.pickedIcon = this.value

                    // bind click event to show the dialog we created above
                    const that = this
                    on(query(this.IconPicker),
                        `click`,
                        function (e) {
                            e.preventDefault()

                            that.showIconDialog()
                        })
                },
                addCssLink(path, id) {
                    const cssId = 'IconPicker' + id
                    if (!document.getElementById(cssId)) { // only if ID isn't found...
                        const head = document.getElementsByTagName('head')[0]
                        const link = document.createElement('link')
                        link.id = cssId
                        link.rel = 'stylesheet'
                        link.type = 'text/css'
                        link.href = path
                        link.media = 'all'
                        head.appendChild(link)
                    }
                },
                createIconDialog() {
                    // get the session. see getSession for more details.
                    const sObject = this.getSession()

                    // create the new dialog wrapper. the list of icons will be injected into .IconPicker-nFontAwesome-list later on.
                    this.dialog = new Dialog({
                        title: `Add Font Awesome Icon`,
                        content: `
        <div class="IconPicker-nFontAwesome-wrapper">
            <div class="IconPicker-nFontAwesome-controls">
                    <div class="IconPicker-nFontAwesome-controls-row">
                        <div class="IconPicker-nFontAwesome-search">
                            <input type="text" placeholder="Search..."></input>
                        </div>
                        <div class="IconPicker-nFontAwesome-counter"><strong>0</strong> <span>icons</span></div>
                    </div>
                    <div class="IconPicker-nFontAwesome-controls-row">
                        <div class="IconPicker-nFontAwesome-styles">
                            ${this.SOLID ? `
                            <label>
                                <input type="checkbox" ${sObject.solid ? `checked="checked"` : ``} name="solid" /> Solid
                            </label>` : ``}
                            ${this.REGULAR ? `
                            <label>
                                <input type="checkbox" ${sObject.regular ? `checked="checked"` : ``} name="regular" /> Regular
                            </label>` : ``}
                            ${this.BRANDS ? `
                            <label>
                                <input type="checkbox" ${sObject.brands ? `checked="checked"` : ``} name="brands" /> Brands
                            </label>` : ``}
                        </div>
                    </div>
                </div>
                <div class="IconPicker-nFontAwesome-list" style="font-size:40px;"></div>
            </div>
        </div>
    `,
                        style: `width:690px;height:500px;`
                    })

                    // setup an input event to the search box within the dialog
                    const that = this
                    const searchBox = query(this.dialog.domNode).query(`.IconPicker-nFontAwesome-search input`)[0]
                    searchBox.addEventListener(`input`, function (e) { that.filterIcons(this, e) }, false)

                    // ...and a change event on the checkboxes.
                    const styleCheckboxes = query(this.dialog.domNode).query(`.IconPicker-nFontAwesome-styles input`)
                    styleCheckboxes.forEach(el => {
                        el.addEventListener(`change`, function (e) { that.changeStyle(this, e) }, false)
                    })
                },
                showIconDialog() {
                    // get our icons
                    this.getIconList()
                    // show the dialog
                    this.dialog.show()
                    // set the search value back to null, if any exists since we want to show all icons on every dialog open
                    query('.IconPicker-nFontAwesome-search').query('input')[0].value = ''
                },
                getIconList() {
                    // get our session cache object
                    const sObject = this.getSession()

                    // if sObject.icons isn't null, we already have icons in the session cache so just return those instead of another call to the api
                    if (sObject.icons) {
                        this.generateListOfIcons()
                        // short circuit the function call
                        return true
                    }

                    const myHeaders = new Headers();
                    myHeaders.append("Content-Type", "application/json");

                    const graphql = JSON.stringify({
                        query: "query { release (version: \"5.13.0\") { icons(license: \"free\") { id label membership {free} } } }",
                        variables: {}
                    })

                    const requestOptions = {
                        method: "POST",
                        headers: myHeaders,
                        body: graphql,
                        redirect: "follow"
                    };


                    // we didn't get short circuited, so no icons in session cache. let's make the api call!
                    fetch('https://api.fontawesome.com', requestOptions)
                        .then(r => r.json())
                        .then(data => {
                            const icons = data.data.release.icons
                            icons.sort((a, b) => a.id.localeCompare(b.id, 'en', { 'sensitivity': 'base' })) // sort icons alphabetically based on ID
                            sObject.icons = icons // add it to our session object
                            this.setSession(sObject) // set the new session object
                            this.generateListOfIcons() // now generate the list of icons for the dialog
                        })
                },
                generateListOfIcons(json) {
                    const that = this
                    const list = query(this.dialog.domNode).query('.IconPicker-nFontAwesome-list')[0] // get handle to list DOM element
                    const counter = query(this.dialog.domNode).query('.IconPicker-nFontAwesome-counter strong')[0] // get handle to our counter DOM element
                    if (list) {
                        // first clear out any html we might have in the list element
                        let html = ''
                        list.innerHTML = html
                        list.classList.add('loading') // add loading class

                        // generate html that we can inject into the list element
                        setTimeout(function () { // this timeout is just to put it at the end of the execution pipeline so it does show the loader etc.
                            const sObject = json || that.getSession() // do we have a filtered list? if not, get it from the session cache object

                            sObject.icons.forEach(icon => {
                                icon.membership.free.forEach(style => {
                                    // get the prefix for the type of icon it is (solid, regular, brand)
                                    const prefix = that.getPrefix(style)
                                    if (prefix && sObject[style]) {
                                        html += `<div data-prefix="${prefix}" data-id="${icon.id}" title="${icon.label}"><span class="${prefix} fa-${icon.id}"></span></div>`
                                    }
                                })
                            })

                            list.innerHTML = html // inject it to our list element
                            list.classList.remove('loading') // remove loading class
                            const icons = list.querySelectorAll('div') // get handle to all icon dom elements
                            counter.innerHTML = icons.length // show how many icons we have showing
                            icons.forEach(icon => { // go through each icon and add a click event listener to select the icon
                                icon.addEventListener('click', function (e) { that.selectIcon(this, e) }, false)
                            })
                        }, 10)
                    }
                },
                getPrefix(style) {
                    let prefix = null

                    if (style === 'solid') {
                        prefix = 'fas'
                    } else if (style === 'regular') {
                        prefix = 'far'
                    } else if (style === 'brands') {
                        prefix = 'fab'
                    }

                    return prefix
                },
                changeStyle(el) {
                    // get the session cache object
                    const sObject = this.getSession()
                    // update sObject type based on if we checked or unchecked the checkbox
                    sObject[el.name] = el.checked
                    // store the object with the new setting
                    this.setSession(sObject)
                    // and regeneate the icon list
                    this.generateListOfIcons(sObject)
                },
                selectIcon(that, e) {
                    const faIcon = `${that.dataset.prefix} fa-${that.dataset.id}` // store our prefix and icon class name
                    this.dialog.hide() // hide the dialog
                    this.onFocus() // without this it won't trigger an update, since the base widget needs to be in focus for this to happen
                    this._setValue(faIcon, true) // set the value of our property
                },
                showSelectedIconInPreview(faIcon) {
                    const preview = query(this.IconPicker).query('span')[0]
                    preview.className = faIcon
                    preview.title = faIcon
                },
                filterIcons(that, e) {
                    const query = that.value
                    const sObject = this.getSession()
                    const results = sObject.icons.filter(icon => { return icon.label.toLowerCase().indexOf(query.toLowerCase()) > -1 })
                    this.generateListOfIcons({
                        ...sObject,
                        icons: results
                    })
                },
                getSession() {
                    // get our object from session storage cache
                    const sObject = sessionStorage.getItem('nFontAwesome')
                    if (sObject) { // if we have an object, just return it
                        return JSON.parse(sObject)
                    } else { // otherwise...
                        // create and return a base object, based on above settings and defaults
                        // this is so we don't get any null ref errors.
                        return {
                            icons: null,
                            solid: this.SOLID,
                            regular: this.REGULAR,
                            brands: this.BRANDS,
                            fontsize: 30
                        }
                    }
                },
                setSession(object) {
                    sessionStorage.setItem('nFontAwesome', JSON.stringify(object))
                },
                _setValue(value, updateTextbox) {
                    // avoids running this if the widget already is started
                    if (this._started && epi.areEqual(this.value, value)) {
                        return
                    }

                    // set value to this widget (and notify observers).
                    this._set('value', value)

                    // set value to tmp value
                    if (updateTextbox) {
                        this.pickedIcon = value
                        // updates our template with the currently selected icon
                        this.showSelectedIconInPreview(this.pickedIcon)
                    }

                    if (this._started && this.validate()) {
                        // Trigger change event
                        this.onChange(value)
                    }
                },
                onChange(value) {
                    // Event
                },
                // just a quick validator that our value is correct and set. in this case that it's not empty or null, pretty much.
                isValid() {
                    return !this.required || (lang.isArray(this.value) && this.value.length > 0 && this.value.join() !== '')
                },
                // Sets the value of the widget to "value" and updates the property
                _setValueAttr(value) {
                    this._setValue(value, true)
                },
                // sets the property to ReadOnly if the PropertyEditorDescriptor has it marked as such.
                _setReadOnlyAttr(value) {
                    this._set('readOnly', value)
                },
                // indicates whether the onChange method is used for each value change or only on demand.
                // set through the PropertyEditorDescriptor.
                _setIntermediateChangesAttr(value) {
                    this._set('intermediateChanges', value)
                },
                _onButtonClick: function () {
                    this.showIconDialog();
                }
            }
        )
    })