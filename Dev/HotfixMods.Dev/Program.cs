/*
 * Console for debugging specific parts of the code and generating stuff.
 * These tools are not part of the compiled software.
 * 
 */

using HotfixMods.Dev.Helpers;

var helper = new ClientDbDefinitionHelper();
await helper.DefinitionToCSharp("ItemSparse", "10.0.0.46112");